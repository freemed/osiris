// created on 25/07/2007 at 12:59 p
////////////////////////////////////////////////////////////
// project created on 24/10/2006 at 10:20 a
// Sistema Hospitalario OSIRIS
// Monterrey - Mexico
//
// Autor    	: Ing. Juan Antonio Peña Gonzalez (Programacion)
//				  Ing. Daniel Olivares (Preprogramacion)
// 				  
// Licencia		: GLP
// S.O. 		: GNU/Linux Ubuntu 6.06 LTS (Dapper Drake)
//////////////////////////////////////////////////////////
//
// proyect osiris is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// proyect osirir is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Foobar; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
//////////////////////////////////////////////////////////
// Programa		: hscmty.cs
// Proposito	: Pagos en Caja 
// Objeto		: cargos_hospitalizacion.cs
//////////////////////////////////////////////////////////	
using System;
using Npgsql;
using System.Data;
using Gtk;
using Glade;

namespace osiris
{
	public class terapia_neonatal
	{
		// Boton general para salir de las ventanas
		// Todas la ventanas en glade este boton debe estra declarado identico
		[Widget] Gtk.Button button_salir;
		
		// Declarando ventana principal de Hospitalizacion
		[Widget] Gtk.Window menu_terapia_neonatal;
		[Widget] Gtk.Button button_cargos_pacientes;
		[Widget] Gtk.Button button_soli_material;
		[Widget] Gtk.Button button_autorizacion_medicamento;
		[Widget] Gtk.Button button_inv_subalmacen;
		[Widget] Gtk.Button button_asignacion_habitacion;
		[Widget] Gtk.Button button_traspaso_subalmacenes;
		
		string LoginEmpleado;
		string NomEmpleado;
		string AppEmpleado;
		string ApmEmpleado;
		string nombrebd;
		
		class_conexion conexion_a_DB = new class_conexion();
		
		public terapia_neonatal (string LoginEmp_, string NomEmpleado_, string AppEmpleado_, string ApmEmpleado_, string _nombrebd_) 
		{
			LoginEmpleado = LoginEmp_;
			NomEmpleado = NomEmpleado_;
			AppEmpleado = AppEmpleado_;
			ApmEmpleado = ApmEmpleado_;
			nombrebd = conexion_a_DB._nombrebd;
			
			Glade.XML gxml = new Glade.XML (null, "terapia_neonatal.glade", "menu_terapia_neonatal", null);
			gxml.Autoconnect (this);
			////// Muestra ventana de Glade
			menu_terapia_neonatal.Show();
			
			////// Sale de la ventana
			button_salir.Clicked += new EventHandler(on_cierraventanas_clicked);
			
			button_cargos_pacientes.Clicked += new EventHandler(on_button_cargos_pacientes_clicked);
			button_soli_material.Clicked += new EventHandler(on_button_soli_material_clicked);
			button_autorizacion_medicamento.Clicked += new EventHandler(on_button_autorizacion_medicamento_clicked);
			button_inv_subalmacen.Clicked += new EventHandler(on_button_inv_subalmacen_clicked);
			button_asignacion_habitacion.Clicked += new EventHandler(on_button_asignacion_habitacion_clicked);
			button_traspaso_subalmacenes.Clicked += new EventHandler(on_button_traspaso_subalmacenes_clicked);
		}
		
		void on_button_cargos_pacientes_clicked(object sender, EventArgs args)
		{
			new osiris.cargos_terapia_neonatal(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd);
		}
		
		void on_button_soli_material_clicked(object sender, EventArgs args)
		{
			new osiris.solicitud_material(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,7);
		}
		
		void on_button_autorizacion_medicamento_clicked(object sender, EventArgs args)
		{
			 new osiris.orden_compra_urgencias(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,7,"UNIDAD CUIDADOS INTENSIVOS NEONATOS",0,"");
		}
		
		void on_button_inv_subalmacen_clicked(object sender, EventArgs args)
		{
			new osiris.inventario_sub_almacen(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,7,"UNIDAD CUIDADOS INTENSIVOS NEONATOS",1);
		}
		
		void on_button_traspaso_subalmacenes_clicked(object sender, EventArgs args)
		{
			new osiris.inventario_sub_almacen(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd,7,"UNIDAD CUIDADOS INTENSIVOS NEONATOS",2);
		}
		
		void on_button_asignacion_habitacion_clicked(object sender, EventArgs args)
		{
		   new osiris.asignacion_de_habitacion(LoginEmpleado,NomEmpleado,AppEmpleado,ApmEmpleado,nombrebd, 0);
		}
		
		void on_cierraventanas_clicked (object sender, EventArgs args)
		{
			Widget win = (Widget) sender;
			win.Toplevel.Destroy();
		}
	}
}