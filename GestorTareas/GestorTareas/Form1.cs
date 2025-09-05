using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GestorTareas
{
    public partial class Form1 : Form
    {
        // Definición de la clase Tarea
        public class Tarea
        {
            public string Codigo { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public DateTime Fecha { get; set; }
            public string Lugar { get; set; }
            public string Estado { get; set; }
        }

        List<Tarea> listaTareas = new List<Tarea>();

        public Form1()
        {
            InitializeComponent();
            InicializarControles();
        }

        private void InicializarControles()
        {
            // Configurar ComboBox de estados
            cmbEstado.Items.AddRange(new object[] { "No realizada", "En proceso", "Terminada" });
            cmbEstado.SelectedIndex = 0;

            // Configurar DataGridView
            dgvTareas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTareas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTareas.MultiSelect = false;
            dgvTareas.ReadOnly = true;
            dgvTareas.AllowUserToAddRows = false;

            // Configurar búsquedas
            cmbBuscarEstado.Items.AddRange(new object[] { "Todos", "No realizada", "En proceso", "Terminada" });
            cmbBuscarEstado.SelectedIndex = 0;
        }

        private void ActualizarGrid()
        {
            dgvTareas.DataSource = null;
            dgvTareas.DataSource = listaTareas;
            ConfigurarColumnasDataGrid();
        }

        private void ConfigurarColumnasDataGrid()
        {
            if (dgvTareas.Columns.Count > 0)
            {
                dgvTareas.Columns["Codigo"].HeaderText = "Código";
                dgvTareas.Columns["Nombre"].HeaderText = "Nombre";
                dgvTareas.Columns["Descripcion"].HeaderText = "Descripción";
                dgvTareas.Columns["Fecha"].HeaderText = "Fecha";
                dgvTareas.Columns["Lugar"].HeaderText = "Lugar";
                dgvTareas.Columns["Estado"].HeaderText = "Estado";

                // Formato para la columna de fecha
                dgvTareas.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }

        private void LimpiarCampos()
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            dtpFecha.Value = DateTime.Now;
            txtLugar.Text = "";
            cmbEstado.SelectedIndex = 0;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txtCodigo.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Los campos Código y Nombre son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verificar si ya existe una tarea con el mismo código
            if (listaTareas.Any(t => t.Codigo == txtCodigo.Text))
            {
                MessageBox.Show("Ya existe una tarea con este código.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Tarea nueva = new Tarea()
            {
                Codigo = txtCodigo.Text,
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text,
                Fecha = dtpFecha.Value,
                Lugar = txtLugar.Text,
                Estado = cmbEstado.SelectedItem.ToString()
            };

            listaTareas.Add(nueva);
            ActualizarGrid();
            LimpiarCampos();
            MessageBox.Show("Tarea agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvTareas.SelectedRows.Count > 0)
            {
                int index = dgvTareas.SelectedRows[0].Index;
                listaTareas[index].Codigo = txtCodigo.Text;
                listaTareas[index].Nombre = txtNombre.Text;
                listaTareas[index].Descripcion = txtDescripcion.Text;
                listaTareas[index].Fecha = dtpFecha.Value;
                listaTareas[index].Lugar = txtLugar.Text;
                listaTareas[index].Estado = cmbEstado.SelectedItem.ToString();

                ActualizarGrid();
                MessageBox.Show("Tarea editada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Seleccione una tarea para editar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvTareas.SelectedRows.Count > 0)
            {
                var confirmacion = MessageBox.Show("¿Está seguro de eliminar esta tarea?", "Confirmar eliminación",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion == DialogResult.Yes)
                {
                    int index = dgvTareas.SelectedRows[0].Index;
                    listaTareas.RemoveAt(index);
                    ActualizarGrid();
                    LimpiarCampos();
                    MessageBox.Show("Tarea eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Seleccione una tarea para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvTareas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtCodigo.Text = dgvTareas.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtNombre.Text = dgvTareas.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtDescripcion.Text = dgvTareas.Rows[e.RowIndex].Cells[2].Value.ToString();
                dtpFecha.Value = (DateTime)dgvTareas.Rows[e.RowIndex].Cells[3].Value;
                txtLugar.Text = dgvTareas.Rows[e.RowIndex].Cells[4].Value.ToString();
                cmbEstado.SelectedItem = dgvTareas.Rows[e.RowIndex].Cells[5].Value.ToString();
            }
        }

        // Funcionalidades de búsqueda
        private void btnBuscarCodigo_Click(object sender, EventArgs e)
        {
            string codigo = txtBuscarCodigo.Text.Trim();
            if (!string.IsNullOrEmpty(codigo))
            {
                var tareaEncontrada = listaTareas.FirstOrDefault(t => t.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));

                if (tareaEncontrada != null)
                {
                    dgvTareas.DataSource = new List<Tarea> { tareaEncontrada };
                    ConfigurarColumnasDataGrid();
                }
                else
                {
                    MessageBox.Show("No se encontró ninguna tarea con ese código.", "Búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                ActualizarGrid();
            }
        }

        private void btnBuscarEstado_Click(object sender, EventArgs e)
        {
            string estado = cmbBuscarEstado.SelectedItem.ToString();

            if (estado == "Todos")
            {
                ActualizarGrid();
            }
            else
            {
                var tareasFiltradas = listaTareas.Where(t => t.Estado == estado).ToList();
                dgvTareas.DataSource = tareasFiltradas;
                ConfigurarColumnasDataGrid();
            }
        }

        private void btnBuscarFecha_Click(object sender, EventArgs e)
        {
            DateTime fechaInicio = dtpFechaInicio.Value.Date;
            DateTime fechaFin = dtpFechaFin.Value.Date.AddDays(1).AddSeconds(-1); // Hasta el final del día

            if (fechaInicio > fechaFin)
            {
                MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha fin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var tareasFiltradas = listaTareas.Where(t => t.Fecha >= fechaInicio && t.Fecha <= fechaFin).ToList();
            dgvTareas.DataSource = tareasFiltradas;
            ConfigurarColumnasDataGrid();
        }

        private void btnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            txtBuscarCodigo.Text = "";
            cmbBuscarEstado.SelectedIndex = 0;
            dtpFechaInicio.Value = DateTime.Now.AddMonths(-1);
            dtpFechaFin.Value = DateTime.Now;
            ActualizarGrid();
        }
    }
}