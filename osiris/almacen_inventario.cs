// created on 26/07/2007 at 03:48 p
///////////////////////////////////////////////////////////
// Hospital Santa Cecilia
// Monterrey - Mexico
//
// Autor    	: ing. Juan Antonio Peña Gonzalez (gjuanzz@gmail.com) 
// 				  Ing. Daniel Olivares C. (Programacion Base y Ajustes)
//
// Licencia		: GLP
//////////////////////////////////////////////////////////
//
// proyect osiris is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// proyect osiris is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Foobar; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
//////////////////////////////////////////////////////////
// Programa		: paquetes_quirurgicos.cs
// Proposito	: Crear paquetes de productos que se aplican en cirugia
// Objeto		: paquetes_quirurgicos.cs
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class inventario_almacen
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir = null;
		
		// Para todas las busquedas este es el nombre asignado
		// se declara una vez
		[Widget] Gtk.Entry entry_expresion = null;
		[Widget] Gtk.Button button_selecciona = null;
		[Widget] Gtk.Button button_buscar_busqueda = null;
		
		// Declarando ventana principal
		[Widget] Gtk.Window inventario = null;
		[Widget] Gtk.Entry entry_id_almacen = null;
		[Widget] Gtk.Entry entry_almacen = null;
		[Widget] Gtk.Entry entry_ano_inventario = null;
		
		//Declaracion de checkbuttons
		[Widget] Gtk.Button button_copia_productos = null;
		
		//Declaracion de radiobuttons
		[Widget] Gtk.ComboBox combobox_mes_inventario = null;
		[Widget] Gtk.TreeView lista_de_inventario = null;
		
		//[Widget] Gtk.ProgressBar progressbar_status_llenado;
		[Widget] Gtk.Button button_quitar_aplicados = null;
		[Widget] Gtk.Button button_actualizar = null;
		[Widget] Gtk.Button button_buscar_almacen = null;
		[Widget] Gtk.Button button_selec_id = null;
		[Widget] Gtk.Button button_graba_inventario = null;
		[Widget] Gtk.Button button_limpiar = null;
		[Widget] Gtk.Button button_busca_producto = null;
		[Widget] Gtk.Button button_reporte = null;
		
		//Declarando la barra de estado
		[Widget] Gtk.Statusbar statusbar_inventario = null;
		
		/////// Ventana Busqueda de productos\\\\\\\\
		//[Widget] Gtk.Window busca_producto;
		[Widget] Gtk.TreeView lista_de_producto = null;
		
		[Widget] Gtk.Entry entry_cantidad_aplicada = null;
		
		///////Ventana de Busqueda de almacenes
		[Widget] Gtk.TreeView lista_almacenes = null;
		
		private TreeStore treeViewEngineBusca;
		private TreeStore treeViewEngineBusca2;
		private TreeStore treeViewEngineInventario;
		
		//private ArrayList arraycargosrealizados;
		
		// Declaracion de variables publicas
		public int idtipoalmacen = 0;	        			// Toma el valor de numero de atencion de paciente
		public string almacen;
		public string mesinventario= "";
		
		public float valoriva = 15;
		
		public string id_produ = "";
		public string desc_produ = "";
		public string costo_unitario_producto;
		public string costo_producto;
		public string embalaje;
		public string precio_produ;
		public string grupo_prod;
		public string grupo1_prod;
		public string grupo2_prod;
		public string constante;
		
		//public float valor_descuento = 0;
		//Variables de admision
		public bool copiaproductos = false;
		public string tipobusqueda = "";
		
		public string LoginEmpleado;
		public string NomEmpleado; 
		public string AppEmpleado; 
		public string ApmEmpleado;
			
		public string connectionString = "Server=localhost;" +
						"Port=5432;" +
						 "User ID=admin;" +
						"Password=1qaz2wsx;";
		public string nombrebd;
				
		public CellRendererText cel_descripcion;
		
		//Declaracion de ventana de error
		protected Gtk.Window MyWinError;
		protected Gtk.Window MyWin;
		
		
		public inventario_almacen(string LoginEmp, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_ ) 
		{
			LoginEmpleado = LoginEmp;
			nombrebd = _nombrebd_; 
			NomEmpleado = NomEmpleado_; 
			AppEmpleado = AppEmpleado_; 
			ApmEmpleado = ApmEmpleado_;
			
			//Console.WriteLine("antes de abrir venmtana");
			Glade.XML gxml = new Glade.XML (null,"almacen_costos_compras.glade","inventario",null);
			gxml.Autoconnect (this);
	        // Muestra ventana de Glade
			inventario.Show();
			
			// Voy a buscar el folio que capturo
			button_selec_id.Clicked += new EventHandler(on_selec_id_clicked);
			// Validando el enter
			entry_id_almacen.KeyPressEvent += onKeyPressEvent_enter_id;
			//Activacion de boton de actualizar
			button_actualizar.Clicked += new EventHandler(on_button_actualizar_clicked);
			// Activacion de boton de busqueda
			button_buscar_almacen.Clicked += new EventHandler(on_button_buscar_almacen_clicked);
			//limpia todos los valores de la ventana
			button_limpiar.Clicked += new EventHandler(on_button_limpiar_clicked);
			// Busqueda de Productos
			button_busca_producto.Clicked += new EventHandler(on_button_busca_producto_clicked);
			//quita lementos aplicados
			button_quitar_aplicados.Clicked += new EventHandler(on_button_quitar_aplicados_clicked);
			// Activacion de grabacion de informacion
	    	button_graba_inventario.Clicked += new EventHandler(on_button_graba_paquete_clicked);
			// Imprime Procedimiento
			button_reporte.Clicked += new EventHandler(on_button_reporte_clicked);
			/*
			//Carga los productos de todo un folio
			button_copia_productos.Clicked += new EventHandler(on_button_copia_procedimiento_clicked);
			//Se activa la opcion de copiar productos
			checkbutton_copia_productos.Clicked += new EventHandler(on_checkbutton_copia_productos_clicked);
			//Se activa la opcion de crear nueva cirugia
			checkbutton_nueva_cirugia.Clicked += new EventHandler(on_checkbutton_nueva_cirugia_clicked);
			*/
			// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			llenado_cmbox_meses();
			// Creacion de los treeview para la pantalla
			crea_treeview_inventario();
			
			treeViewEngineInventario.Clear();
			//entry_id_cirugia.Sensitive = true;
			entry_almacen.Sensitive = false;
			entry_ano_inventario.Sensitive = false;
			button_graba_inventario.Sensitive = true;
			button_reporte.Sensitive = true;
			button_quitar_aplicados.Sensitive = true;
			button_limpiar.Sensitive = true;
			lista_de_inventario.Sensitive = true;
			
			combobox_mes_inventario.Sensitive = false;
			button_busca_producto.Sensitive = false;
			button_graba_inventario.Sensitive = false;
			button_reporte.Sensitive = false;
			button_quitar_aplicados.Sensitive = false;
			button_limpiar.Sensitive = false;
			button_copia_productos.Sensitive = false;
			
			statusbar_inventario.Pop(0);
			statusbar_inventario.Push(1, "login: "+LoginEmpleado+"  |Usuario: "+NomEmpleado+" "+AppEmpleado+" "+ApmEmpleado);
			statusbar_inventario.HasResizeGrip = false;
	    	
			// pone color a los entry
			entry_id_almacen.ModifyBase(StateType.Normal, new Gdk.Color(54,180,221));
			entry_almacen.ModifyBase(StateType.Normal, new Gdk.Color(254,253,152));
		}
		
		void llenado_cmbox_meses()
		{
			combobox_mes_inventario.Clear();
			CellRendererText cell1 = new CellRendererText();
			combobox_mes_inventario.PackStart(cell1, true);
			combobox_mes_inventario.AddAttribute(cell1,"text",0);
	        
			ListStore store1 = new ListStore( typeof (string),typeof (string));
			combobox_mes_inventario.Model = store1;
	        
			store1.AppendValues ("","0");
			store1.AppendValues ("ENERO","1");
			store1.AppendValues ("FEBRERO","2");
			store1.AppendValues ("MARZO","3");
			store1.AppendValues ("ABRIL","4");
			store1.AppendValues ("MAYO","5");
			store1.AppendValues ("JUNIO","6");
			store1.AppendValues ("JULIO","7");
			store1.AppendValues ("AGOSTO","8");
			store1.AppendValues ("SEPTIEMBRE","9");
			store1.AppendValues ("OCTUBRE","10");
			store1.AppendValues ("NOVIEMBRE","11");
			store1.AppendValues ("DICIEMBRE","12");
	        	              
			TreeIter iter1;
			if (store1.GetIterFirst(out iter1)){
				combobox_mes_inventario.SetActiveIter (iter1);
			}
			combobox_mes_inventario.Changed += new EventHandler (onComboBoxChanged_mes_inventario);
		}
		
		void onComboBoxChanged_mes_inventario (object sender, EventArgs args)
		{
	    	//Console.WriteLine("cambio el combobox");
	    	if(entry_ano_inventario.Text.Trim() != "" )   
			{
		    	ComboBox combobox_mes_inventario = sender as ComboBox;
				if (sender == null)	{	return;	}
		  		TreeIter iter;
		  		if (combobox_mes_inventario.GetActiveIter (out iter))
			    {
			    	mesinventario = (string) combobox_mes_inventario.Model.GetValue(iter,1);
			    	llenado_de_material_aplicado(idtipoalmacen.ToString()); 
		    	}
		    }else{
		    	MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close, " selecione un año de inventario \n");
				msgBoxError.Run ();			msgBoxError.Destroy();
		    }
		}
				
		void on_selec_id_clicked(object sender, EventArgs args)
		{
			if(entry_id_almacen.Text  == "" || entry_id_almacen.Text == " "){
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error,ButtonsType.Close, 
				"Debe de llenar el campo de id almacen con uno \n"+"existente para que los datos se muestren \n");
				msgBoxError.Run ();			msgBoxError.Destroy();
			}else{
				//Console.WriteLine ("on_selec_id selecciono el ID");
				idtipoalmacen = int.Parse(entry_id_almacen.Text.ToString());
				llenado_de_almacen(entry_id_almacen.Text );
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza la ventana de
		// carga de almacen
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent_enter_id(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				if(entry_id_almacen.Text.Trim()  == "" ){
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
					MessageType.Error,ButtonsType.Close, 
					"Debe de llenar el campo de id almacen con uno \n"+"existente para que los datos se muestren ");
					msgBoxError.Run ();			msgBoxError.Destroy();
				}else{
					//Console.WriteLine ("actualiza selecciono el ID");
					idtipoalmacen = int.Parse(entry_id_almacen.Text.ToString());
					//tipo_de_inventario();
					llenado_de_almacen(entry_id_almacen.Text );
				}			
			}
			string misDigitos = "0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				//Console.WriteLine(Convert.ToChar(args.Event.Key));
				args.RetVal = true;
			}
		}
		
		void on_button_actualizar_clicked(object sender, EventArgs args)
		{
			idtipoalmacen = int.Parse(entry_id_almacen.Text.ToString());
			llenado_de_material_aplicado(idtipoalmacen.ToString());
		}
														
		void llenado_de_almacen(string idalmacen)
		{
			treeViewEngineInventario.Clear();
			//entry_id_cirugia.Sensitive = true;
			entry_almacen.Sensitive = true;
			entry_ano_inventario.Sensitive = true;
			entry_ano_inventario.GrabFocus();
			combobox_mes_inventario.Sensitive = true;
			button_graba_inventario.Sensitive = true;
			button_reporte.Sensitive = true;
			button_quitar_aplicados.Sensitive = true;
			button_limpiar.Sensitive = true;
			lista_de_inventario.Sensitive = true;
			button_copia_productos.Sensitive = true;
			
			NpgsqlConnection conexion;
			conexion = new NpgsqlConnection (connectionString+nombrebd);
	           
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
	              	
				comando.CommandText = "SELECT * "+
									"FROM hscmty_almacenes  "+
					            	"WHERE id_almacen = '"+idalmacen.ToString()+"' ;";
				//Console.WriteLine("query llenado cirugia: "+comando.CommandText.ToString());				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				
				if(lector.Read())
				{
					idtipoalmacen = int.Parse(idalmacen);
					entry_almacen.Text = (string) lector["descripcion_almacen"];
				}
			}catch (NpgsqlException ex){
		   		Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   		MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
							MessageType.Error, ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
				msgBoxError.Run (); 	msgBoxError.Destroy();
		    }
	       	conexion.Close ();
       	}
		
		// llenando el detalle de procedimiento de cobranza
		void llenado_de_material_aplicado(string idalmacen)
		{	
			//Console.WriteLine("entro a llenar lista de productos");
			treeViewEngineInventario.Clear();
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
			            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
				
				comando.CommandText = "SELECT descripcion_producto, "+
							"id_quien_creo,hscmty_productos.aplicar_iva,hscmty_inventario_almacenes.id_almacen,  "+
							"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto, "+
							"to_char(hscmty_inventario_almacenes.id_producto,'999999999999') AS idproducto,"+
							"to_char(hscmty_inventario_almacenes.stock,'999999.99') AS stock, "+
							"to_char(hscmty_inventario_almacenes.id_secuencia,'999999999999') AS idsecuencia,"+
							"eliminado,"+
							//"to_char(hscmty_catalogo_almacenes."+mesinventario.ToString()+",'99999.99') AS stock, "+
							"to_char(hscmty_productos.costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(hscmty_productos.costo_producto,'999999999.99') AS costoproducto, "+
							"to_char(hscmty_productos.precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(hscmty_productos.cantidad_de_embalaje,'999999999.99') AS embalaje, "+
							"to_char(hscmty_inventario_almacenes.fechahora_alta,'dd-MM-yyyy HH:mi:ss') AS fechcreacion "+
							"FROM "+
							"hscmty_inventario_almacenes,hscmty_productos,hscmty_almacenes,hscmty_grupo_producto,hscmty_grupo1_producto,hscmty_grupo2_producto "+
							"WHERE hscmty_inventario_almacenes.id_producto = hscmty_productos.id_producto "+ 
							"AND hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
							"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
							"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto "+
							"AND hscmty_inventario_almacenes.id_almacen = hscmty_almacenes.id_almacen "+
							"AND hscmty_inventario_almacenes.id_almacen = '"+(string) idalmacen +"' "+
							"AND hscmty_inventario_almacenes.ano_inventario = '"+(string) entry_ano_inventario.Text +"' "+
							"AND hscmty_inventario_almacenes.mes_inventario = '"+mesinventario+"' "+
							"AND eliminado = 'false' "+
							"ORDER BY to_char(hscmty_inventario_almacenes.fechahora_alta,'dd-MM-yyyy HH:mi:ss');";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				//Console.WriteLine("query llenado de materiales: "+comando.CommandText.ToString());
				while (lector.Read()){
					if(float.Parse((string) lector["stock"]) != 0)
					{
							treeViewEngineInventario.AppendValues ((string) lector["idproducto"],
																	(string) lector["descripcion_producto"],
																	(string) lector["stock"],
																	(string) lector["costoproductounitario"],
																	(string) lector["costoproducto"],
																	(string) lector["embalaje"],
																	(string) lector["preciopublico"],
																	(string) lector["descripcion_grupo_producto"],
																	(string) lector["descripcion_grupo1_producto"],
																	(string) lector["descripcion_grupo2_producto"],
																	(string) lector["id_quien_creo"],
																	(string) lector["fechcreacion"],	
																	true,
																	(string) lector["idsecuencia"] );
					}
					
				}
				button_busca_producto.Sensitive = true;
				// Realizando las restas
			}catch (NpgsqlException ex) { Console.WriteLine ("PostgresSQL error: {0}",ex.Message);	}
			conexion.Close ();
		}
		
		void on_button_buscar_almacen_clicked(object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "busca_tipo_almacen", null);
			gxml.Autoconnect (this);
			
			tipobusqueda ="almacenes";
			// Activa la salida de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			// Activa la seleccion de cirugia
			button_selecciona.Clicked += new EventHandler(on_selecciona_almacen_clicked);
			// Activa boton de busqueda
			button_buscar_busqueda.Clicked += new EventHandler(on_button_llena_almacenes_clicked);
	       	//funcion para que aplique el enter
	       	entry_expresion.KeyPressEvent += click_enter;
	       	
	       	treeViewEngineBusca = new TreeStore( typeof(int), typeof(string));
			lista_almacenes.Model = treeViewEngineBusca;
			
			lista_almacenes.RulesHint = true;
			
			lista_almacenes.RowActivated += on_selecciona_almacen_clicked;  // Doble click selecciono paciente
			
			TreeViewColumn col_idalmacen = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idalmacen.Title = "ID Almacen"; // titulo de la cabecera de la columna, si está visible
			col_idalmacen.PackStart(cellr0, true);
			col_idalmacen.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			
			TreeViewColumn col_descripalmacen = new TreeViewColumn();
			CellRendererText cellrt1 = new CellRendererText();
			col_descripalmacen.Title = "Descripcion de Almacen";
			col_descripalmacen.PackStart(cellrt1, true);
			col_descripalmacen.AddAttribute (cellrt1, "text", 1); // la siguiente columna será 1 en vez de 2
			
			lista_almacenes.AppendColumn(col_idalmacen);
			lista_almacenes.AppendColumn(col_descripalmacen);
		}
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void click_enter(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llena_lista_de_busqueda();			
			}
		}
		
		void on_button_llena_almacenes_clicked(object sender, EventArgs args)
		{
			llena_lista_de_busqueda();
		}
		
		void llena_lista_de_busqueda() 
		{
			treeViewEngineBusca.Clear();// Limpia el treeview cuando realiza una nueva busqueda
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
           	// Verifica que la base de datos este conectada
			try{
					conexion.Open ();
					NpgsqlCommand comando; 
					comando = conexion.CreateCommand ();
		           	if ((string) entry_expresion.Text.ToUpper() == "*")	{
						comando.CommandText ="SELECT * FROM hscmty_almacenes "+
											" ORDER BY id_almacen;";
					}else{
						comando.CommandText ="SELECT * FROM hscmty_almacenes "+
											"WHERE descripcion_almacen LIKE '%"+entry_expresion.Text.ToUpper()+"%' "+
											" ORDER BY id_almacen;";
					}
					NpgsqlDataReader lector = comando.ExecuteReader ();
					while (lector.Read())	{
						treeViewEngineBusca.AppendValues ((int) lector["id_almacen"],(string) lector["descripcion_almacen"]);//TreeIter iter =
					}
				}catch (NpgsqlException ex) { Console.WriteLine ("PostgresSQL error: {0}",ex.Message); }
			conexion.Close ();
		}
		
		void on_selecciona_almacen_clicked (object sender, EventArgs args)
		{
			TreeModel model;			TreeIter iterSelected;
			if (lista_almacenes.Selection.GetSelected(out model, out iterSelected)) {
				idtipoalmacen = (int) model.GetValue(iterSelected, 0);
				almacen = (string) model.GetValue(iterSelected, 1);
				entry_id_almacen.Text = idtipoalmacen.ToString(); 
				entry_almacen.Text = almacen;
				llenado_de_almacen(idtipoalmacen.ToString());
				// cierra la ventana despues que almaceno la informacion en variables
				Widget win = (Widget) sender;
				win.Toplevel.Destroy();
			}
		}
		
		void on_button_limpiar_clicked(object sender, EventArgs args)
		{
			//limpia_valores();
			this.entry_id_almacen.Text = "";
			this.entry_almacen.Text = "";
			this.entry_ano_inventario.Text = "";
			this.button_reporte.Sensitive = false;
			treeViewEngineInventario.Clear();
			this.llenado_cmbox_meses();
			combobox_mes_inventario.Sensitive = false;
			button_busca_producto.Sensitive = false;
			button_graba_inventario.Sensitive = false;
			button_reporte.Sensitive = false;
			button_quitar_aplicados.Sensitive = false;
			button_copia_productos.Sensitive = false;
			button_limpiar.Sensitive = false;
		}
		
		void on_button_busca_producto_clicked (object sender, EventArgs args)
		{
			Glade.XML gxml = new Glade.XML (null, "almacen_costos_compras.glade", "busca_producto", null);
			gxml.Autoconnect (this);
			crea_treeview_busqueda();
			entry_expresion.KeyPressEvent += onKeyPressEvent_enter_expresion;
			button_buscar_busqueda.Clicked += new EventHandler(on_llena_lista_producto_clicked);
			button_selecciona.Clicked += new EventHandler(on_selecciona_producto_clicked);
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			entry_cantidad_aplicada.KeyPressEvent += onKeyPressEvent;
		}
		
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		void onKeyPressEvent_enter_expresion(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter){
				args.RetVal = true;
				llenando_busqueda_productos();
			}
		}
		
		// Valida entradas que solo sean numericas, se utiliza eb ventana de
		// carga de producto
		[GLib.ConnectBefore ()]   	  // Esto es indispensable para que funcione    
		public void onKeyPressEvent(object o, Gtk.KeyPressEventArgs args)
		{
			//Console.WriteLine(args.Event.Key);
			//Console.WriteLine(Convert.ToChar(args.Event.Key));
			string misDigitos = "-.0123456789ﾰﾱﾲﾳﾴﾵﾶﾷﾸﾹﾮｔｒｓｑ（";
			if (Array.IndexOf(misDigitos.ToCharArray(), Convert.ToChar(args.Event.Key)) == -1 && args.Event.Key != Gdk.Key.BackSpace)
			{
				args.RetVal = true;
			}
		}
		
		void on_llena_lista_producto_clicked (object sender, EventArgs args)
 		{
 			llenando_busqueda_productos();
 		}
 			
 		void llenando_busqueda_productos()
 		{
 			treeViewEngineBusca2.Clear(); // Limpia el treeview cuando realiza una nueva busqueda
			
			NpgsqlConnection conexion; 
			conexion = new NpgsqlConnection (connectionString+nombrebd);
            
			// Verifica que la base de datos este conectada
			try{
				conexion.Open ();
				NpgsqlCommand comando; 
				comando = conexion.CreateCommand ();
               	
				comando.CommandText = "SELECT to_char(hscmty_productos.id_producto,'999999999999') AS codProducto,"+
							"hscmty_productos.descripcion_producto, "+
							"descripcion_grupo_producto,descripcion_grupo1_producto,descripcion_grupo2_producto, "+
							"to_char(precio_producto_publico,'99999999.99') AS preciopublico,"+
							"to_char(costo_por_unidad,'999999999.99') AS costoproductounitario, "+
							"to_char(costo_producto,'999999999.99') AS costoproducto, "+
							"to_char(cantidad_de_embalaje,'999999999.99') AS embalaje "+
							"FROM hscmty_productos,hscmty_grupo_producto,hscmty_grupo1_producto,hscmty_grupo2_producto "+
							"WHERE hscmty_productos.id_grupo_producto = hscmty_grupo_producto.id_grupo_producto "+
							"AND hscmty_productos.id_grupo1_producto = hscmty_grupo1_producto.id_grupo1_producto "+
							"AND hscmty_productos.id_grupo2_producto = hscmty_grupo2_producto.id_grupo2_producto "+
							"AND hscmty_grupo_producto.agrupacion4 = 'ALM' "+
							"AND hscmty_productos.cobro_activo = 'true' "+
							"AND hscmty_productos.descripcion_producto LIKE '%"+entry_expresion.Text.ToUpper().Trim()+"%' ORDER BY descripcion_producto; ";
				
				NpgsqlDataReader lector = comando.ExecuteReader ();
				while (lector.Read())
				{
					treeViewEngineBusca2.AppendValues ((string) lector["codProducto"] , 
														(string) lector["descripcion_producto"],
														//stock
														(string) lector["costoproductounitario"],
														(string) lector["costoproducto"],
														(string) lector["embalaje"],
														(string) lector["preciopublico"],
														(string) lector["descripcion_grupo_producto"],
														(string) lector["descripcion_grupo1_producto"],
														(string) lector["descripcion_grupo2_producto"]);
				}
			}catch (NpgsqlException ex) { Console.WriteLine ("PostgresSQL error: {0}",ex.Message); }
			conexion.Close ();
		}
		
		void on_selecciona_producto_clicked (object sender, EventArgs args)
		{
			TreeModel model;
			TreeIter iterSelected;

 			if (lista_de_producto.Selection.GetSelected(out model, out iterSelected)) 
 			{
 				//Console.WriteLine("pasavalor");
 				id_produ = (string) model.GetValue(iterSelected, 0);//0(string) lector["codProducto"] ,
				desc_produ = (string) model.GetValue(iterSelected, 1);//1(string) lector["descripcion_producto"],
				costo_unitario_producto = (string) model.GetValue(iterSelected, 2);//2(string) lector["preciopublico"],
				costo_producto = (string) model.GetValue(iterSelected, 3);//3calculodeiva.ToString("F").PadLeft(10),
				embalaje = (string) model.GetValue(iterSelected, 4);//4preciomasiva.ToString("F").PadLeft(10),
				precio_produ  = (string) model.GetValue(iterSelected, 5); //8(string) lector["costoproductounitario"],
				grupo_prod = (string) model.GetValue(iterSelected, 6);//9(string) lector["porcentageutilidad"],
				grupo1_prod = (string) model.GetValue(iterSelected, 7);//10(string) lector["costoproducto"],
				grupo2_prod = (string) model.GetValue(iterSelected,8);
				constante = entry_cantidad_aplicada.Text;
					
				if ((float) float.Parse(constante) > 0){
					treeViewEngineInventario.AppendValues ((string) id_produ,
												(string) desc_produ,
												(string) constante,
												(string) costo_unitario_producto,
												(string) costo_producto,
												(string) embalaje,
												(string) precio_produ,
												(string) grupo_prod,
												(string) grupo1_prod,
												(string) grupo2_prod,
												(string) LoginEmpleado,
												(string) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
												(bool) false,
												(string) "0");
					entry_cantidad_aplicada.Text = "0";
					entry_expresion.Text = "";
					entry_expresion.GrabFocus();
				}else{
					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
												MessageType.Error,ButtonsType.Close, 
											"La cantidad que quiere aplicar debe ser \n"+"distinta a cero, intente de nuevo");
					msgBoxError.Run ();					msgBoxError.Destroy();
				}
			}
		}
		
		void on_button_quitar_aplicados_clicked(object sender, EventArgs args)
		{
			TreeIter iter;
 			TreeModel model;
 			string prodeliminado;
 			if (lista_de_inventario.Selection.GetSelected (out model, out iter)) {
 				if (!(bool) lista_de_inventario.Model.GetValue (iter,12)){
					
					treeViewEngineInventario.Remove (ref iter);
					
 				}else{
 					prodeliminado = (string) lista_de_inventario.Model.GetValue (iter,1);
 					if (LoginEmpleado =="DOLIVARES" || LoginEmpleado =="HVARGAS" || LoginEmpleado =="JPENA" ){
 						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Question,ButtonsType.YesNo,"¿ Desea eliminar "+prodeliminado+" del paquete este producto ?");
						ResponseType miResultado = (ResponseType)
						msgBox.Run ();				msgBox.Destroy();
	 			
	 					if (miResultado == ResponseType.Yes){
	 						NpgsqlConnection conexion; 
							conexion = new NpgsqlConnection (connectionString+nombrebd);
			    			// Verifica que la base de datos este conectada
							try{
								conexion.Open ();
								NpgsqlCommand comando; 
								comando = conexion.CreateCommand ();
				 				comando.CommandText = "UPDATE hscmty_inventario_almacenes "+
										"SET eliminado = 'true' , "+
										"fechahora_eliminado = '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', "+
										"id_quien_elimino = '"+LoginEmpleado+"' "+								
				 						"WHERE id_secuencia = '"+(string) lista_de_inventario.Model.GetValue (iter,13)+"' ;";
										comando.ExecuteNonQuery();
					        			comando.Dispose();
			        			
			        			
			        			msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
										MessageType.Info,ButtonsType.Ok,"El Producto "+prodeliminado.ToString()+" se devolvio satisfactoriamente");
								msgBox.Run ();					msgBox.Destroy();
								
								this.llenado_de_material_aplicado((string) entry_id_almacen.Text );
								
								conexion.Close ();
			        		}catch (NpgsqlException ex){
				   				Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
				   		
							}
 						}
 					}else{
 						MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
						MessageType.Error,ButtonsType.Ok,"No esta autorizado para esta opcion...");
						msgBox.Run();
						msgBox.Destroy();
 					}
 				}
 			}
		}
		
		
		void on_button_graba_paquete_clicked(object sender, EventArgs args)
		{
			if(entry_almacen.Text.Trim() == "" || entry_ano_inventario.Text == "" || this.mesinventario == "" ||
			   entry_id_almacen.Text.Trim()  == "" || entry_ano_inventario.Text == "") 
			{
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
											ButtonsType.Close,"verifique que no existan campos en blanco");
				msgBoxError.Run ();					msgBoxError.Destroy();
			}else{
				MessageDialog msgBox = new MessageDialog (MyWin,DialogFlags.Modal,
									MessageType.Question,ButtonsType.YesNo,
									"¿ Desea grabar o actualizar esta infomacion ?");
				ResponseType miResultado = (ResponseType)
				msgBox.Run ();			msgBox.Destroy();
 				if (miResultado == ResponseType.Yes){
					guarda_productos();
					entry_id_almacen.Text = idtipoalmacen.ToString();
					llenado_de_almacen(idtipoalmacen.ToString());
					this.llenado_de_material_aplicado(idtipoalmacen.ToString());
				}else{///termina el proceso de nuevacirugia == false
	 				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
	 											ButtonsType.Close, "No grabo, ya que NO CARGO NADA");
					msgBoxError.Run ();			msgBoxError.Destroy();
	 			}
	 		}
		}
		
		void guarda_productos()
		{
			TreeIter iter;
			//Console.WriteLine("guarda PRODUCTOS a la cirugia: "+idtipocirugia);
			if ((int) this.idtipoalmacen > 0){  // Validando que seleccione un folio de atencion
				if (treeViewEngineInventario.GetIterFirst (out iter)){
					if ((bool)lista_de_inventario.Model.GetValue (iter,12) == false){
						NpgsqlConnection conexion; 
						conexion = new NpgsqlConnection (connectionString+nombrebd);
						// Verifica que la base de datos este conectada
						try{
							conexion.Open ();
							NpgsqlCommand comando; 
							comando = conexion.CreateCommand ();
							//Console.WriteLine("leeo primer linea"+(string) lista_de_servicios.Model.GetValue(iter,2));
							comando.CommandText = "INSERT INTO hscmty_inventario_almacenes("+
														"id_producto,"+
														"id_almacen,"+
														"stock,"+
														"mes_inventario,"+
														"ano_inventario,"+
														"fechahora_alta,"+
														"id_quien_creo) "+
														"VALUES ('"+
														(string) lista_de_inventario.Model.GetValue(iter,0)+"','"+
														idtipoalmacen+"','"+
														(string) lista_de_inventario.Model.GetValue(iter,2)+"','"+
														this.mesinventario+"','"+
														float.Parse(entry_ano_inventario.Text)+"','"+
														DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
														LoginEmpleado+"');";
							//Console.WriteLine(comando.CommandText);
							comando.ExecuteNonQuery();
							comando.Dispose();
						
							while (treeViewEngineInventario.IterNext(ref iter)){
				   				if ((bool)lista_de_inventario.Model.GetValue (iter,12) == false){
									//Console.WriteLine("leeo primer linea"+(string) lista_de_servicios.Model.GetValue(iter,2));
									comando.CommandText = "INSERT INTO hscmty_inventario_almacenes("+
														"id_producto,"+
														"id_almacen,"+
														"stock,"+
														"mes_inventario,"+
														"ano_inventario,"+
														"fechahora_alta,"+
														"id_quien_creo) "+
														"VALUES ('"+
														(string) lista_de_inventario.Model.GetValue(iter,0)+"','"+
														idtipoalmacen+"','"+
														(string) lista_de_inventario.Model.GetValue(iter,2)+"','"+
														this.mesinventario+"','"+
														float.Parse(entry_ano_inventario.Text)+"','"+
														DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+
														LoginEmpleado+"');";
									//Console.WriteLine(comando.CommandText);
									comando.ExecuteNonQuery();
									comando.Dispose();
								}
							}
							MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,MessageType.Info,
			 							ButtonsType.Close, "Los datos se guardaron con EXITO");
							msgBoxError.Run ();			msgBoxError.Destroy();
						}catch (NpgsqlException ex){
		   					Console.WriteLine ("PostgresSQL error: {0}",ex.Message);
		   					MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
										MessageType.Info,ButtonsType.Close, "PostgresSQL error: {0} ",ex.Message);
							msgBoxError.Run ();					msgBoxError.Destroy();
						}
						conexion.Close ();
					}
				}
			}
		}
		
		void on_button_reporte_clicked(object sender, EventArgs args)
		{
			if ((string) this.entry_id_almacen.Text.Trim() == "" )
		    {	
				MessageDialog msgBoxError = new MessageDialog (MyWinError,DialogFlags.DestroyWithParent,
				MessageType.Error, 
				ButtonsType.Close, "Debe de llenar el campo de almacen con uno \n"+
							"existente para que el  se muestre \n"+"o no a pulsado el boton ''Seleccionar''");
				msgBoxError.Run ();
				msgBoxError.Destroy();
			}else{
				//new osiris.inventario_almacen_reporte (int.Parse(entry_id_almacen.Text.ToString()),entry_almacen.Text,mesinventario,entry_ano_inventario.Text,
				//									LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
			}
		}
				
		void crea_treeview_inventario()
		{
			treeViewEngineInventario = new TreeStore(typeof(string),//0 ID
													typeof(string),//1 Descripcion
													typeof(string),//2stock
													typeof(string),//3costo unit
													typeof(string),//4costo prod
													typeof(string),//5embalaje
													typeof(string),//6precio publico
													typeof(string),//7grupo
													typeof(string),//8grupo1
													typeof(string),//9grupo2
													typeof(string),//10quien lo creo
													typeof(string),//11fecha creacion
													typeof(bool),//12 cargado
													typeof(string)); // 13 id_secuencia
			lista_de_inventario.Model = treeViewEngineInventario;
			lista_de_inventario.RulesHint = true;
				
			TreeViewColumn col_idproducto = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idproducto.Title = "ID Producto"; // titulo de la cabecera de la columna, si está visible
			col_idproducto.PackStart(cellr0, true);
			col_idproducto.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			col_idproducto.SortColumnId = (int) Column_inv.col_idproducto;
			col_idproducto.SetCellDataFunc(cellr0, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_desc_producto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_desc_producto.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
			col_desc_producto.PackStart(cellr1, true);
			col_desc_producto.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
			col_desc_producto.SortColumnId = (int) Column_inv.col_desc_producto;
			col_desc_producto.SetCellDataFunc(cellr1, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
            TreeViewColumn col_stock = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_stock.Title = "stock";
			col_stock.PackStart(cellrt2, true);
			col_stock.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
			col_stock.SortColumnId = (int) Column_inv.col_stock;
			col_stock.SetCellDataFunc(cellrt2,new Gtk.TreeCellDataFunc(cambia_colores_fila));
            
			TreeViewColumn col_precioprod = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_precioprod.Title = "Costo Unit.";
			col_precioprod.PackStart(cellrt3, true);
			col_precioprod.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 1 en vez de 2
			col_precioprod.SortColumnId = (int) Column_inv.col_precioprod;
			col_precioprod.SetCellDataFunc(cellrt3,new Gtk.TreeCellDataFunc(cambia_colores_fila));
           
           	TreeViewColumn col_costo_prod = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_costo_prod.Title = "Costo Prod";
			col_costo_prod.PackStart(cellrt4, true);
			col_costo_prod.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 7 en vez de 8
			col_costo_prod.SortColumnId = (int) Column_inv.col_costo_prod;
			col_costo_prod.SetCellDataFunc(cellrt4, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_embalaje = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_embalaje.Title = "Embalaje";
			col_embalaje.PackStart(cellrt5, true);
			col_embalaje.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 7 en vez de 8
			col_embalaje.SortColumnId = (int) Column_inv.col_embalaje;
			col_embalaje.SetCellDataFunc(cellrt5, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_precio_pub = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_precio_pub.Title = "Precio Publico";
			col_precio_pub.PackStart(cellrt6, true);
			col_precio_pub.AddAttribute (cellrt6, "text", 6); // la siguiente columna será 7 en vez de 8
			col_precio_pub.SortColumnId = (int) Column_inv.col_precio_pub;
           	col_precio_pub.SetCellDataFunc(cellrt6, new Gtk.TreeCellDataFunc(cambia_colores_fila));
           
			TreeViewColumn col_grupoprod = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_grupoprod.Title = "Grupo Producto";
			col_grupoprod.PackStart(cellrt7, true);
			col_grupoprod.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 7 en vez de 8
			col_grupoprod.SortColumnId = (int) Column_inv.col_grupoprod;
			col_grupoprod.SetCellDataFunc(cellrt7, new Gtk.TreeCellDataFunc(cambia_colores_fila));
          
			TreeViewColumn col_grupo1prod = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_grupo1prod.Title = "Grupo1 Producto";
			col_grupo1prod.PackStart(cellrt8, true);
			col_grupo1prod.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 8 en vez de 9
			col_grupo1prod.SortColumnId = (int) Column_inv.col_grupo1prod;
			col_grupo1prod.SetCellDataFunc(cellrt8, new Gtk.TreeCellDataFunc(cambia_colores_fila));
                        
			TreeViewColumn col_grupo2prod = new TreeViewColumn();
			CellRendererText cellrt9 = new CellRendererText();
			col_grupo2prod.Title = "Grupo2 Producto";
			col_grupo2prod.PackStart(cellrt9, true);
			col_grupo2prod.AddAttribute (cellrt9, "text", 9); // la siguiente columna será 8 en vez de 9
			col_grupo2prod.SortColumnId = (int) Column_inv.col_grupo2prod;
			col_grupo2prod.SetCellDataFunc(cellrt9, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			TreeViewColumn col_fecha_hora = new TreeViewColumn();
			CellRendererText cellrt10 = new CellRendererText();
			col_fecha_hora.Title = "Fecha de creacion";
			col_fecha_hora.PackStart(cellrt10, true);
			col_fecha_hora.AddAttribute (cellrt10, "text", 10); // la siguiente columna será 8 en vez de 9
			col_fecha_hora.SortColumnId = (int) Column_inv.col_fecha_hora;
			col_fecha_hora.SetCellDataFunc(cellrt10, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			
			TreeViewColumn col_empleado = new TreeViewColumn();
			CellRendererText cellrt11 = new CellRendererText();
			col_empleado.Title = "Empleado";
			col_empleado.PackStart(cellrt11, true);
			col_empleado.AddAttribute (cellrt11, "text", 11); // la siguiente columna será 8 en vez de 9
			col_empleado.SortColumnId = (int) Column_inv.col_empleado;
			col_empleado.SetCellDataFunc(cellrt11, new Gtk.TreeCellDataFunc(cambia_colores_fila));
			
			lista_de_inventario.AppendColumn(col_idproducto);  // 0
			lista_de_inventario.AppendColumn(col_desc_producto); // 1
			lista_de_inventario.AppendColumn(col_stock);//2
			lista_de_inventario.AppendColumn(col_precioprod);//3	
			lista_de_inventario.AppendColumn(col_costo_prod); //4
			lista_de_inventario.AppendColumn(col_embalaje);  //5
			lista_de_inventario.AppendColumn(col_precio_pub);//6
			lista_de_inventario.AppendColumn(col_grupoprod);	//7
			lista_de_inventario.AppendColumn(col_grupo1prod);	//8
			lista_de_inventario.AppendColumn(col_grupo2prod);	//9	
			lista_de_inventario.AppendColumn(col_fecha_hora);//10
			lista_de_inventario.AppendColumn(col_empleado);//11
		}
		
		enum Column_inv
		{
			col_idproducto,
			col_desc_producto,
			col_stock,
			col_precioprod,
			col_costo_prod,
			col_embalaje,
			col_precio_pub,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod,
			col_fecha_hora,
			col_empleado
		}
		
		void crea_treeview_busqueda()
		{
			treeViewEngineBusca2 = new TreeStore(typeof(string),//0
												typeof(string),//1
												typeof(string),//2
												typeof(string),//3
												typeof(string),//4
												typeof(string),//5
												typeof(string),//6
												typeof(string),//7
												typeof(string));//8
			lista_de_producto.Model = treeViewEngineBusca2;
			lista_de_producto.RulesHint = true;
			
			lista_de_producto.RowActivated += on_selecciona_producto_clicked;  // Doble click selecciono paciente
				
			TreeViewColumn col_idproducto = new TreeViewColumn();
			CellRendererText cellr0 = new CellRendererText();
			col_idproducto.Title = "ID Producto"; // titulo de la cabecera de la columna, si está visible
			col_idproducto.PackStart(cellr0, true);
			col_idproducto.AddAttribute (cellr0, "text", 0);    // la siguiente columna será 1 en vez de 1
			col_idproducto.SortColumnId = (int) Column_prod.col_idproducto;
			
			TreeViewColumn col_desc_producto = new TreeViewColumn();
			CellRendererText cellr1 = new CellRendererText();
			col_desc_producto.Title = "Descripcion de Producto"; // titulo de la cabecera de la columna, si está visible
			col_desc_producto.PackStart(cellr1, true);
			col_desc_producto.AddAttribute (cellr1, "text", 1);    // la siguiente columna será 1 en vez de 1
			col_desc_producto.SortColumnId = (int) Column_prod.col_desc_producto;
			//cellr0.Editable = true;   // Permite edita este campo
            
			TreeViewColumn col_precioprod = new TreeViewColumn();
			CellRendererText cellrt2 = new CellRendererText();
			col_precioprod.Title = "Costo Unit.";
			col_precioprod.PackStart(cellrt2, true);
			col_precioprod.AddAttribute (cellrt2, "text", 2); // la siguiente columna será 1 en vez de 2
			col_precioprod.SortColumnId = (int) Column_prod.col_precioprod;
           
           	TreeViewColumn col_costo_prod = new TreeViewColumn();
			CellRendererText cellrt3 = new CellRendererText();
			col_costo_prod.Title = "Costo Prod";
			col_costo_prod.PackStart(cellrt3, true);
			col_costo_prod.AddAttribute (cellrt3, "text", 3); // la siguiente columna será 7 en vez de 8
			col_costo_prod.SortColumnId = (int) Column_prod.col_costo_prod;
			
			TreeViewColumn col_embalaje = new TreeViewColumn();
			CellRendererText cellrt4 = new CellRendererText();
			col_embalaje.Title = "Embalaje";
			col_embalaje.PackStart(cellrt4, true);
			col_embalaje.AddAttribute (cellrt4, "text", 4); // la siguiente columna será 7 en vez de 8
			col_embalaje.SortColumnId = (int) Column_prod.col_embalaje;
			
			TreeViewColumn col_precio_pub = new TreeViewColumn();
			CellRendererText cellrt5 = new CellRendererText();
			col_precio_pub.Title = "Precio Publico";
			col_precio_pub.PackStart(cellrt5, true);
			col_precio_pub.AddAttribute (cellrt5, "text", 5); // la siguiente columna será 7 en vez de 8
			col_precio_pub.SortColumnId = (int) Column_prod.col_precio_pub;
           
			TreeViewColumn col_grupoprod = new TreeViewColumn();
			CellRendererText cellrt6 = new CellRendererText();
			col_grupoprod.Title = "Grupo Producto";
			col_grupoprod.PackStart(cellrt6, true);
			col_grupoprod.AddAttribute (cellrt6, "text", 6); // la siguiente columna será 7 en vez de 8
			col_grupoprod.SortColumnId = (int) Column_prod.col_grupoprod;
          
			TreeViewColumn col_grupo1prod = new TreeViewColumn();
			CellRendererText cellrt7 = new CellRendererText();
			col_grupo1prod.Title = "Grupo1 Producto";
			col_grupo1prod.PackStart(cellrt7, true);
			col_grupo1prod.AddAttribute (cellrt7, "text", 7); // la siguiente columna será 8 en vez de 9
			col_grupo1prod.SortColumnId = (int) Column_prod.col_grupo1prod;
                        
			TreeViewColumn col_grupo2prod = new TreeViewColumn();
			CellRendererText cellrt8 = new CellRendererText();
			col_grupo2prod.Title = "Grupo2 Producto";
			col_grupo2prod.PackStart(cellrt8, true);
			col_grupo2prod.AddAttribute (cellrt8, "text", 8); // la siguiente columna será 8 en vez de 9
			col_grupo2prod.SortColumnId = (int) Column_prod.col_grupo2prod;
			
			lista_de_producto.AppendColumn(col_idproducto);  // 0
			lista_de_producto.AppendColumn(col_desc_producto); // 1
			lista_de_producto.AppendColumn(col_precioprod);	//2
			lista_de_producto.AppendColumn(col_costo_prod);//3
			lista_de_producto.AppendColumn(col_embalaje);   //4
			lista_de_producto.AppendColumn(col_precio_pub);//5
			lista_de_producto.AppendColumn(col_grupoprod);	//6
			lista_de_producto.AppendColumn(col_grupo1prod);	//7
			lista_de_producto.AppendColumn(col_grupo2prod);	//8						
		}
		
		enum Column_prod
		{
			col_idproducto,
			col_desc_producto,
			col_precioprod,
			col_costo_prod,
			col_embalaje,
			col_precio_pub,
			col_grupoprod,
			col_grupo1prod,
			col_grupo2prod
		}
		
		//ACCION QUE CAMBIA EL COLOR DEL TEXTO PARA CUANDO SE GUARDA EN LA BASE DE DATOS 
		void cambia_colores_fila(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//descripcion_producto descrip = (descripcion_producto) model.GetValue (iter, 14);
			if ((bool)lista_de_inventario.Model.GetValue (iter,12)==true){
				(cell as Gtk.CellRendererText).Foreground = "darkblue";
			}else{
				(cell as Gtk.CellRendererText).Foreground = "red";
			}
		}

		// cierra ventanas emergentes
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
		
	}
}
