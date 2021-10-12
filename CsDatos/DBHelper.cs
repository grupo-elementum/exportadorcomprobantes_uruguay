using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CsDatos {
	/// <summary>
	/// Provee los métodos necesarios para interactuar con la base de datos
	/// </summary>
	public class DBHelper {
		#region StoredProcedureParameter

		/// <summary>
		/// Representa a un parámetro de un stored procedure
		/// </summary>
		public class StoredProcedureParameter {
			private string _name = "";
			private object _value = null;
			private System.Data.ParameterDirection _direction = ParameterDirection.Input;
			
			/// <summary>
			/// Construye un parámetro a partir de su nombre, el valor y la dirección
			/// </summary>
			/// <param name="name">El nombre del parámetro</param>
			/// <param name="value">El valor del parámetro</param>
			/// <param name="direction">Indica si el parámetro es de entrada, salida o ambos</param>
			public StoredProcedureParameter(string name, object value, System.Data.ParameterDirection direction) {
				_name = name;
				_value = value;
				_direction = direction;
			}

			/// <summary>
			/// Construye el parámetro a partir de su nombre y el valor
			/// </summary>
			/// <param name="name">El nombre del parámetro</param>
			/// <param name="value">El valor del parámetro</param>
			public StoredProcedureParameter(string name, object value) {
				_name = name;
				_value = value;
				_direction = ParameterDirection.Input;
			}

			/// <summary>
			/// Construye el parámetro a partir de su nombre
			/// </summary>
			/// <param name="name">El nombre del parámetro</param>
			public StoredProcedureParameter(string name) {
				_name = name;
				_value = DBNull.Value;
				_direction = ParameterDirection.Input;
			}

			/// <summary>
			/// Devuelve o establece el nombre del parámetro
			/// </summary>
			public string Name {
				get {
					return _name;
				}
				set {
					_name = value;
				}
			}

			/// <summary>
			/// Devuelve o establece el valor del parámetro
			/// </summary>
			public object Value {
				get {
					return _value;
				}
				set {
					_value = value;
				}
			}

			/// <summary>
			/// Devuelve o establece la dirección del parámetro
			/// </summary>
			public System.Data.ParameterDirection Direction {
				get {
					return _direction;
				}
				set {
					_direction = value;
				}
			}

			/// <summary>
			/// Devuelve el parámetro creado a partir del proveedor que se esté usando
			/// </summary>
			/// <returns>Un objeto que implementa IDataParameter</returns>
			public IDataParameter GetParameterObject() {
				SqlParameter parameter = null;
				parameter = new SqlParameter(_name, _value);
				if (!parameter.ParameterName.StartsWith("@"))
					parameter.ParameterName = string.Format("@{0}", parameter.ParameterName);

				parameter.Direction = _direction;
				return parameter;
			}
		}

		#endregion

		#region Private Members

		private SqlConnection _connection;
		
		#endregion

		#region Constructors

		/// <summary>
		/// Crea un objeto SqlHelper con una cadena de conexión en particular
		/// </summary>
		/// <param name="connectionString">La cadena de conexión a la base de datos a utilizar</param>
		public DBHelper(string connectionString) {
			_connection = new SqlConnection(connectionString);
		}

		/// <summary>
		/// Crea un objeto SqlHelper
		/// </summary>
		public DBHelper() {
            Conexion oCon = Conexion.Instance;
			//string connectionString = ConfigurationSettings.AppSettings["ConnectionString"];
            string connectionString = oCon.get_connectionstring();
            //string connectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
			_connection = new SqlConnection(connectionString);
		}
        public DBHelper(int a)
        {
            //string connectionString = ConfigurationSettings.AppSettings["ConnectionStringEntrada"];
            Conexion oCon = Conexion.Instance;
            string connectionString = oCon.get_connectionstring_entrada();
            _connection = new SqlConnection(connectionString);
        }
		#endregion

		#region Properties

		/// <summary>
		/// Devuelve la conexión actual
		/// </summary>
		public System.Data.IDbConnection Connection {
			get {
				return _connection;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Realiza la conexión a la base de datos
		/// </summary>
		public void Connect() {
			try {
				_connection.Open();
			}
			catch (Exception ex) {
				throw ex;
			}
		}

		/// <summary>
		/// Cierra la conexión a la base de datos
		/// </summary>
		public void Disconnect() {
			try {
				if (_connection.State != ConnectionState.Closed) {
					_connection.Close();
				}
			}
			catch (Exception ex) {
				throw ex;
			}
		}

		#region Execute Procedure Methods

		#region ExecuteProcedureAsScalar

		/// <summary>
		/// Ejecuta un stored procedure devolviendo un valor
		/// </summary>
		/// <param name="procedure">El nombre del stored procedure</param>
		/// <returns>El valor que retorna el stored procedure</returns>
		public object ExecuteProcedureAsScalar(string procedure) {
			return ExecuteProcedureAsScalar(procedure, null, null);
		}

		/// <summary>
		/// Ejecuta un stored procedure enviando parámetros devolviendo un valor
		/// </summary>
		/// <param name="procedure">El nombre del stored procedure</param>
		/// <param name="parameters">La colección de parámetros que hay que pasarle al stored procedure</param>
		/// <returns>El valor que retorna el stored procedure</returns>
		public object ExecuteProcedureAsScalar(string procedure, StoredProcedureParameter[] parameters) {
			return ExecuteProcedureAsScalar(procedure, parameters, null);
		}

		/// <summary>
		/// Ejecuta un stored procedure enviando parámetros dentro de una transacción devolviendo un valor
		/// </summary>
		/// <param name="procedure">El nombre del stored procedure</param>
		/// <param name="parameters">La colección de parámetros que hay que pasarle al stored procedure</param>
		/// <param name="transaction">La transacción a utilizar para llamar al stored procedure</param>
		/// <returns>El valor que retorna el stored procedure</returns>
		
		public object ExecuteProcedureAsScalar(string procedure, StoredProcedureParameter[] parameters, IDbTransaction transaction) {
			object retval = null;
			bool wasOpen = (_connection.State == ConnectionState.Open);

			try {
				SqlCommand cmd = new SqlCommand(procedure, _connection);
				if (! wasOpen) {
					_connection.Open();
				}

				cmd.CommandType = CommandType.StoredProcedure;

				if (parameters != null) {
					foreach (StoredProcedureParameter parameter in parameters) {
						cmd.Parameters.Add(parameter.GetParameterObject());
					}
				}

				if (transaction != null) {
					cmd.Transaction = (SqlTransaction) transaction;
				}

				retval = cmd.ExecuteScalar();
			}
			catch (Exception ex) {
				throw ex;
			}
			finally {
				if (!wasOpen && _connection.State != ConnectionState.Closed) {
					_connection.Close();
				}
			}

			return retval;
		}

		#endregion

		#region ExecuteProcedureAsReader

		/// <summary>
		/// Ejecuta un stored procedure devolviendo una referencia a un IDataReader
		/// </summary>
		/// <param name="procedure">El nombre del stored procedure</param>
		/// <returns>Una referencia a un objeto IDataReader</returns>
		public IDataReader ExecuteProcedureAsReader(string procedure) {
			return ExecuteProcedureAsReader(procedure, null, null);
		}

		/// <summary>
		/// Ejecuta un stored procedure enviando parámetros devolviendo 
		/// una referencia a un IDataReader
		/// </summary>
		/// <param name="procedure">El nombre del stored procedure</param>
		/// <param name="parameters">La colección de parámetros que hay que pasarle al stored procedure</param>
		/// <returns>Una referencia a un objeto IDataReader</returns>
		public IDataReader ExecuteProcedureAsReader(string procedure, StoredProcedureParameter[] parameters) {
			return ExecuteProcedureAsReader(procedure, parameters, null);
		}

		/// <summary>
		/// Ejecuta un stored procedure enviando parámetros dentro de una transacción devolviendo 
		/// una referencia a un IDataReader
		/// </summary>
		/// <param name="procedure">El nombre del stored procedure</param>
		/// <param name="parameters">La colección de parámetros que hay que pasarle al stored procedure</param>
		/// <param name="transaction">La transacción a utilizar para llamar al stored procedure</param>
		/// <returns>Una referencia a un objeto IDataReader</returns>
		public IDataReader ExecuteProcedureAsReader(string procedure, StoredProcedureParameter[] parameters, IDbTransaction transaction) {
			SqlDataReader retval = null;

			try {
				SqlCommand cmd = new SqlCommand(procedure, _connection);

				if (_connection.State != ConnectionState.Open) {
					_connection.Open();
				}

				cmd.CommandType = CommandType.StoredProcedure;

				if (parameters != null) {
					foreach (StoredProcedureParameter parameter in parameters) {
						cmd.Parameters.Add(parameter.GetParameterObject());
					}
				}

				if (transaction != null) {
					cmd.Transaction = (SqlTransaction) transaction;
				}

				retval = cmd.ExecuteReader();
			}
			catch (Exception ex) {
				throw ex;
			}

			return retval;
		}

		#endregion

		#region ExecuteProcedureNonQuery

		/// <summary>
		/// Ejecuta un stored procedure que no devuelve ningún valor
		/// </summary>
		/// <param name="procedure">El nombre del stored procedure</param>
		public void ExecuteProcedureNonQuery(string procedure) {
			ExecuteProcedureNonQuery(procedure, null, null);
		}

		/// <summary>
		/// Ejecuta un stored procedure enviando parámetros que no devuelve ningún valor
		/// </summary>
		/// <param name="procedure">El nombre del stored procedure</param>
		/// <param name="parameters">La colección de parámetros que hay que pasarle al stored procedure</param>
		public void ExecuteProcedureNonQuery(string procedure, StoredProcedureParameter[] parameters) {
			ExecuteProcedureNonQuery(procedure, parameters, null);
		}

		/// <summary>
		/// Ejecuta un stored procedure enviando parámetros dentro de una transacción que no devuelve ningún valor
		/// </summary>
		/// <param name="procedure">El nombre del stored procedure</param>
		/// <param name="parameters">La colección de parámetros que hay que pasarle al stored procedure</param>
		/// <param name="transaction">La transacción a utilizar para llamar al stored procedure</param>
		public void ExecuteProcedureNonQuery(string procedure, StoredProcedureParameter[] parameters, IDbTransaction transaction) {
			try {
				SqlCommand cmd = new SqlCommand(procedure, _connection);

				if (_connection.State != ConnectionState.Open) {
					_connection.Open();
				}

				cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

				if (parameters != null) {
					foreach (StoredProcedureParameter parameter in parameters) {
						cmd.Parameters.Add(parameter.GetParameterObject());
					}
				}

				if (transaction != null) {
					cmd.Transaction = (SqlTransaction) transaction;
				}

				cmd.ExecuteNonQuery();
			}
			catch (Exception ex) {
				throw ex;
			}
		}

		#endregion

		#region ExecuteProcedureAsDataSet

		/// <summary>
		/// Ejecuta un stored procedure enviando devolviendo una referencia a un objeto DataSet
		/// </summary>
		/// <param name="procedure">El nombre del stored procedure</param>
		/// <returns>Una referencia a un objeto DataSet</returns>
		public DataSet ExecuteProcedureAsDataSet(string procedure) {
			return ExecuteProcedureAsDataSet(procedure, null, null);
		}

		/// <summary>
		/// Ejecuta un stored procedure enviando parámetros devolviendo 
		/// una referencia a un objeto DataSet
		/// </summary>
		/// <param name="procedure">El nombre del stored procedure</param>
		/// <param name="parameters">La colección de parámetros que hay que pasarle al stored procedure</param>
		/// <returns>Una referencia a un objeto DataSet</returns>
		public DataSet ExecuteProcedureAsDataSet(string procedure, StoredProcedureParameter[] parameters) {
			return ExecuteProcedureAsDataSet(procedure, parameters, null);
		}

		/// <summary>
		/// Ejecuta un stored procedure enviando parámetros dentro de una transacción devolviendo 
		/// una referencia a un objeto DataSet
		/// </summary>
		/// <param name="procedure">El nombre del stored procedure</param>
		/// <param name="parameters">La colección de parámetros que hay que pasarle al stored procedure</param>
		/// <param name="transaction">La transacción a utilizar para llamar al stored procedure</param>
		/// <returns>Una referencia a un objeto DataSet</returns>
		public DataSet ExecuteProcedureAsDataSet(string procedure, StoredProcedureParameter[] parameters, IDbTransaction transaction) {
			DataSet retval = new DataSet();
			bool wasOpen = (_connection.State == ConnectionState.Open);

			try {
				SqlCommand cmd = new SqlCommand(procedure, _connection);

				if (! wasOpen) {
					_connection.Open();
				}

				cmd.CommandType = CommandType.StoredProcedure;

				if (parameters != null) {
					foreach (StoredProcedureParameter parameter in parameters) {
						cmd.Parameters.Add(parameter.GetParameterObject());
					}
				}

				if (transaction != null) {
					cmd.Transaction = (SqlTransaction) transaction;
				}

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(retval);
			}
			catch (Exception ex) {
				throw ex;
			}
			finally {
				if (!wasOpen && _connection.State != ConnectionState.Closed) {
					_connection.Close();
				}
			}

			return retval;
		}

		#endregion

		#region ExecuteProcedureAsDataTable

		/// <summary>
		/// Ejecuta un stored procedure enviando parámetros dentro de una transacción devolviendo 
		/// una referencia a un objeto DataTable
		/// </summary>
		/// <param name="procedure">El nombre del stored procedure</param>
		/// <returns>Una referencia a un objeto DataTable</returns>
		public DataTable ExecuteProcedureAsDataTable(string procedure) {
			return ExecuteProcedureAsDataTable(procedure, null, null);
		}

		/// <summary>
		/// Ejecuta un stored procedure enviando parámetros dentro de una transacción devolviendo 
		/// una referencia a un objeto DataTable
		/// </summary>
		/// <param name="procedure">El nombre del stored procedure</param>
		/// <param name="parameters">La colección de parámetros que hay que pasarle al stored procedure</param>
		/// <returns>Una referencia a un objeto DataTable</returns>
		public DataTable ExecuteProcedureAsDataTable(string procedure, StoredProcedureParameter[] parameters) {
			return ExecuteProcedureAsDataTable(procedure, parameters, null);
		}

		/// <summary>
		/// Ejecuta un stored procedure enviando parámetros dentro de una transacción devolviendo 
		/// una referencia a un objeto DataTable
		/// </summary>
		/// <param name="procedure">El nombre del stored procedure</param>
		/// <param name="parameters">La colección de parámetros que hay que pasarle al stored procedure</param>
		/// <param name="transaction">La transacción a utilizar para llamar al stored procedure</param>
		/// <returns>Una referencia a un objeto DataTable</returns>
		public DataTable ExecuteProcedureAsDataTable(string procedure, StoredProcedureParameter[] parameters, IDbTransaction transaction) {
			DataTable retval = new DataTable();
			bool wasOpen = (_connection.State == ConnectionState.Open);

			try {
				SqlCommand cmd = new SqlCommand(procedure, _connection);

				if (! wasOpen) {
					_connection.Open();
				}

				cmd.CommandType = CommandType.StoredProcedure;

				if (parameters != null) {
					foreach (StoredProcedureParameter parameter in parameters) {
						cmd.Parameters.Add(parameter.GetParameterObject());
					}
				}

				if (transaction != null) {
					cmd.Transaction = (SqlTransaction) transaction;
				}

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(retval);
			}
			catch (Exception ex) {
				throw ex;
			}
			finally {
				if (!wasOpen && _connection.State != ConnectionState.Closed) {
					_connection.Close();
				}
			}
			return retval;
		}

		#endregion

		#endregion

		#region Execute SQL Methods

		#region ExecuteSqlAsScalar

		/// <summary>
		/// Ejecuta una sentencia SQL devolviendo un valor
		/// </summary>
		/// <param name="sql">La sentencia SQL a ejecutar</param>
		/// <returns>El valor que retorna la sentencia SQL ejecutada</returns>
		public object ExecuteSqlAsScalar(string sql) {
			return ExecuteSqlAsScalar(sql, null);
		}

		/// <summary>
		/// Ejecuta una sentencia SQL dentro de una transacción devolviendo un valor
		/// </summary>
		/// <param name="sql">La sentencia SQL a ejecutar</param>
		/// <param name="transaction">La transacción a utilizar para ejecutar la sentencia SQL</param>
		/// <returns>El valor que retorna la sentencia SQL ejecutada</returns>
		public object ExecuteSqlAsScalar(string sql, IDbTransaction transaction) {
			bool wasOpen = (_connection.State == ConnectionState.Open);

			try {
				SqlCommand cmd = new SqlCommand(sql, _connection);
				if (! wasOpen) {
					_connection.Open();
				}

				cmd.CommandType = CommandType.Text;

				if (transaction != null) {
					cmd.Transaction = (SqlTransaction) transaction;
				}

				return cmd.ExecuteScalar();
			}
			catch (Exception ex) {
				throw ex;
			}
			finally {
				if (!wasOpen && _connection.State != ConnectionState.Closed) {
					_connection.Close();
				}
			}
		}

		#endregion

		#region ExecuteSqlAsReader

		/// <summary>
		/// Ejecuta una sentencia SQL devolviendo una referencia a un IDataReader
		/// </summary>
		/// <param name="sql">La sentencia SQL a ejecutar</param>
		/// <returns>Una referencia a un objeto IDataReader</returns>
		public IDataReader ExecuteSqlAsReader(string sql) {
			return ExecuteSqlAsReader(sql, null);
		}

		/// <summary>
		/// Ejecuta una sentencia SQL dentro de una transacción devolviendo 
		/// una referencia a un IDataReader
		/// </summary>
		/// <param name="sql">La sentencia SQL a ejecutar</param>
		/// <param name="transaction">La transacción a utilizar para ejecutar la sentencia SQL</param>
		/// <returns>Una referencia a un objeto IDataReader</returns>
		public IDataReader ExecuteSqlAsReader(string sql, IDbTransaction transaction) {
			SqlDataReader retval = null;

			try {
				SqlCommand cmd = new SqlCommand(sql, _connection);

				if (_connection.State != ConnectionState.Open) {
					_connection.Open();
				}

				cmd.CommandType = CommandType.Text;

				if (transaction != null) {
					cmd.Transaction = (SqlTransaction) transaction;
				}

				retval = cmd.ExecuteReader();
			}
			catch (Exception ex) {
				throw ex;
			}

			return retval;
		}

		#endregion

		#region ExecuteSqlNonQuery

		/// <summary>
		/// Ejecuta una sentencia SQL dentro de una transacción que no devuelve ningún valor
		/// </summary>
		/// <param name="sql">La sentencia SQL a ejecutar</param>
		public void ExecuteSqlNonQuery(string sql) {
			ExecuteSqlNonQuery(sql, null);
		}

		/// <summary>
		/// Ejecuta una sentencia SQL dentro de una transacción que no devuelve ningún valor
		/// </summary>
		/// <param name="sql">La sentencia SQL a ejecutar</param>
		/// <param name="transaction">La transacción a utilizar para ejecutar la sentencia SQL</param>
		public void ExecuteSqlNonQuery(string sql, IDbTransaction transaction) {
			try {
				SqlCommand cmd = new SqlCommand(sql, _connection);

				if (_connection.State != ConnectionState.Open) {
					_connection.Open();
				}

				cmd.CommandType = CommandType.Text;

				if (transaction != null) {
					cmd.Transaction = (SqlTransaction) transaction;
				}

				cmd.ExecuteNonQuery();
			}
			catch (Exception ex) {
				throw ex;
			}
		}

		#endregion

		#region ExecuteSqlAsDataSet

		/// <summary>
		/// Ejecuta una sentencia SQL dentro devolviendo una referencia a un objeto DataSet
		/// </summary>
		/// <param name="sql">La sentencia SQL a ejecutar</param>
		/// <returns>Una referencia a un objeto DataSet</returns>
		public DataSet ExecuteSqlAsDataSet(string sql) {
			return ExecuteSqlAsDataSet(sql, null);
		}

		/// <summary>
		/// Ejecuta una sentencia SQL dentro de una transacción devolviendo 
		/// una referencia a un objeto DataSet
		/// </summary>
		/// <param name="sql">La sentencia SQL a ejecutar</param>
		/// <param name="transaction">La transacción a utilizar para ejecutar la sentencia SQL</param>
		/// <returns>Una referencia a un objeto DataSet</returns>
		public DataSet ExecuteSqlAsDataSet(string sql, IDbTransaction transaction) {
			DataSet retval = new DataSet();
			bool wasOpen = (_connection.State == ConnectionState.Open);

			try {
				SqlCommand cmd = new SqlCommand(sql, _connection);

				if (! wasOpen) {
					_connection.Open();
				}

				cmd.CommandType = CommandType.Text;

				if (transaction != null) {
					cmd.Transaction = (SqlTransaction) transaction;
				}

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(retval);

			}
			catch (Exception ex) {
				throw ex;
			}
			finally {
				if (!wasOpen && _connection.State != ConnectionState.Closed) {
					_connection.Close();
				}
			}

			return retval;
		}

		#endregion

		#region ExecuteSqlAsDataTable

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sql">La sentencia SQL a ejecutar</param>
		/// <returns>Una referencia a un objeto DataTable</returns>
		public DataTable ExecuteSqlAsDataTable(string sql) {
			return ExecuteSqlAsDataTable(sql, null);
		}

		/// <summary>
		/// Ejecuta una sentencia SQL dentro de una transacción devolviendo 
		/// una referencia a un objeto DataTable
		/// </summary>
		/// <param name="sql">La sentencia SQL a ejecutar</param>
		/// <param name="transaction">La transacción a utilizar para ejecutar la sentencia SQL</param>
		/// <returns>Una referencia a un objeto DataTable</returns>
		public DataTable ExecuteSqlAsDataTable(string sql, IDbTransaction transaction) {
			DataTable retval = new DataTable();
			bool wasOpen = (_connection.State == ConnectionState.Open);

			try {
				SqlCommand cmd = new SqlCommand(sql, _connection);

				if (! wasOpen) {
					_connection.Open();
				}

				cmd.CommandType = CommandType.Text;

				if (transaction != null) {
					cmd.Transaction = (SqlTransaction) transaction;
				}

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(retval);
			}
			catch (Exception ex) {
				throw ex;
			}
			finally {
				if (!wasOpen && _connection.State != ConnectionState.Closed) {
					_connection.Close();
				}
			}
			return retval;
		}

		#endregion

		#endregion

		#endregion
	}
}

