using System;
using System.Globalization;
using System.Windows.Forms;

namespace CalculatorApp
{
    public partial class Calculator : Form
    {
        private double currentValue = 0;
        private double memoryValue = 0;
        private string currentOperation = "";
        private bool newOperation = true;
        private bool operationPending = false;

        public Calculator()
        {
            InitializeComponent();
            AttachEventHandlers();
        }

        private void AttachEventHandlers()
        {
            // Botones numéricos
            btn0.Click += (s, e) => AppendNumber("0");
            btn1.Click += (s, e) => AppendNumber("1");
            btn2.Click += (s, e) => AppendNumber("2");
            btn3.Click += (s, e) => AppendNumber("3");
            btn4.Click += (s, e) => AppendNumber("4");
            btn5.Click += (s, e) => AppendNumber("5");
            btn6.Click += (s, e) => AppendNumber("6");
            btn7.Click += (s, e) => AppendNumber("7");
            btn8.Click += (s, e) => AppendNumber("8");
            btn9.Click += (s, e) => AppendNumber("9");

            // Operaciones básicas
            btnAdd.Click += (s, e) => SetOperation("+");
            btnSubtract.Click += (s, e) => SetOperation("-");
            btnMultiply.Click += (s, e) => SetOperation("*");
            btnDivide.Click += (s, e) => SetOperation("/");

            // Botones especiales
            btnEquals.Click += (s, e) => PerformCalculation();
            btnDecimal.Click += (s, e) => AddDecimalPoint();
            btnClear.Click += (s, e) => ClearCalculator();
            btnBack.Click += (s, e) => Backspace();
            btnSign.Click += (s, e) => ChangeSign();
            btnPercent.Click += (s, e) => CalculatePercentage();

            // Memoria
            btnMplus.Click += (s, e) => MemoryAdd();
            btnMminus.Click += (s, e) => MemorySubtract();
            btnMC.Click += (s, e) => MemoryClear();
            btnMR.Click += (s, e) => MemoryRecall();

            // Funciones avanzadas
            btnExp.Click += (s, e) => SetOperation("^");
            btnSqrt.Click += (s, e) => CalculateSquareRoot();
            btnPi.Click += (s, e) => ShowPi();
            btnLog.Click += (s, e) => CalculateLog();
            btnReciprocal.Click += (s, e) => CalculateReciprocal();
            btnSquare.Click += (s, e) => CalculateSquare();

            // Nuevas funciones especiales (botones rojos)
            btnSin.Click += (s, e) => CalculateSin();
            btnCos.Click += (s, e) => CalculateCos();
            btnTan.Click += (s, e) => CalculateTan();
            btnFactorial.Click += (s, e) => CalculateFactorial();
        }

        private void AppendNumber(string number)
        {
            if (newOperation)
            {
                txtDisplay.Text = "";
                newOperation = false;
            }

            // Evitar múltiples ceros a la izquierda
            if (txtDisplay.Text == "0" && number != ".")
                txtDisplay.Text = "";

            txtDisplay.Text += number;
        }

        private void SetOperation(string op)
        {
            if (operationPending)
            {
                PerformCalculation();
            }
            else
            {
                currentValue = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);
            }

            currentOperation = op;
            operationPending = true;
            newOperation = true;
        }

        private void PerformCalculation()
        {
            if (!operationPending) return;

            double newValue = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);

            switch (currentOperation)
            {
                case "+": currentValue += newValue; break;
                case "-": currentValue -= newValue; break;
                case "*": currentValue *= newValue; break;
                case "/":
                    if (newValue != 0)
                        currentValue /= newValue;
                    else
                        MessageBox.Show("No se puede dividir por cero");
                    break;
                case "^": currentValue = Math.Pow(currentValue, newValue); break;
            }

            txtDisplay.Text = currentValue.ToString(CultureInfo.InvariantCulture);
            operationPending = false;
            newOperation = true;
        }

        private void AddDecimalPoint()
        {
            if (newOperation)
            {
                txtDisplay.Text = "0.";
                newOperation = false;
            }
            else if (!txtDisplay.Text.Contains("."))
            {
                txtDisplay.Text += ".";
            }
        }

        private void ClearCalculator()
        {
            txtDisplay.Text = "0";
            currentValue = 0;
            currentOperation = "";
            newOperation = true;
            operationPending = false;
        }

        private void Backspace()
        {
            if (txtDisplay.Text.Length > 1)
            {
                txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
            }
            else
            {
                txtDisplay.Text = "0";
                newOperation = true;
            }
        }

        private void ChangeSign()
        {
            if (txtDisplay.Text != "0")
            {
                double value = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);
                txtDisplay.Text = (-value).ToString(CultureInfo.InvariantCulture);
            }
        }

        private void CalculatePercentage()
        {
            if (operationPending && !string.IsNullOrEmpty(currentOperation))
            {
                // En el contexto de una operación pendiente, calcular el porcentaje del primer valor
                double firstValue = currentValue;
                double percentageValue = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);

                switch (currentOperation)
                {
                    case "+":
                        currentValue = firstValue + (firstValue * percentageValue / 100);
                        break;
                    case "-":
                        currentValue = firstValue - (firstValue * percentageValue / 100);
                        break;
                    case "*":
                        currentValue = firstValue * (percentageValue / 100);
                        break;
                    case "/":
                        if (percentageValue != 0)
                            currentValue = firstValue / (percentageValue / 100);
                        else
                            MessageBox.Show("No se puede dividir por cero");
                        break;
                    default:
                        // Para otras operaciones, usar el comportamiento original
                        currentValue = percentageValue / 100;
                        break;
                }

                txtDisplay.Text = currentValue.ToString(CultureInfo.InvariantCulture);
                operationPending = false;
                newOperation = true;
            }
            else
            {
                // Comportamiento original cuando no hay operación pendiente
                double value = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);
                txtDisplay.Text = (value / 100).ToString(CultureInfo.InvariantCulture);
            }
        }

        private void MemoryAdd() => memoryValue += double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);
        private void MemorySubtract() => memoryValue -= double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);
        private void MemoryClear() => memoryValue = 0;
        private void MemoryRecall() => txtDisplay.Text = memoryValue.ToString(CultureInfo.InvariantCulture);

        private void CalculateSquareRoot()
        {
            double value = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);
            if (value >= 0)
                txtDisplay.Text = Math.Sqrt(value).ToString(CultureInfo.InvariantCulture);
            else
                MessageBox.Show("No se puede calcular la raíz de un número negativo");
        }

        private void ShowPi() => txtDisplay.Text = Math.PI.ToString(CultureInfo.InvariantCulture);

        private void CalculateLog()
        {
            double value = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);
            if (value > 0)
                txtDisplay.Text = Math.Log10(value).ToString(CultureInfo.InvariantCulture);
            else
                MessageBox.Show("No se puede calcular el logaritmo de un número menor o igual a cero");
        }

        private void CalculateReciprocal()
        {
            double value = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);
            if (value != 0)
                txtDisplay.Text = (1 / value).ToString(CultureInfo.InvariantCulture);
            else
                MessageBox.Show("No se puede dividir por cero");
        }

        private void CalculateSquare()
        {
            double value = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);
            txtDisplay.Text = (value * value).ToString(CultureInfo.InvariantCulture);
        }

        // Nuevas funciones especiales (botones rojos)
        private void CalculateSin()
        {
            double value = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);
            // Convertir a radianes si está en grados
            double radians = value * Math.PI / 180;
            txtDisplay.Text = Math.Sin(radians).ToString(CultureInfo.InvariantCulture);
            newOperation = true;
        }

        private void CalculateCos()
        {
            double value = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);
            // Convertir a radianes si está en grados
            double radians = value * Math.PI / 180;
            txtDisplay.Text = Math.Cos(radians).ToString(CultureInfo.InvariantCulture);
            newOperation = true;
        }

        private void CalculateTan()
        {
            double value = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);
            // Convertir a radianes si está en grados
            double radians = value * Math.PI / 180;

            // Evitar valores donde la tangente es indefinida
            if (Math.Cos(radians) == 0)
            {
                MessageBox.Show("La tangente no está definida para este ángulo");
                return;
            }

            txtDisplay.Text = Math.Tan(radians).ToString(CultureInfo.InvariantCulture);
            newOperation = true;
        }

        private void CalculateFactorial()
        {
            double value = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);

            // Solo calcular factorial para números enteros no negativos
            if (value < 0 || value != Math.Floor(value))
            {
                MessageBox.Show("El factorial solo está definido para números enteros no negativos");
                return;
            }

            if (value == 0 || value == 1)
            {
                txtDisplay.Text = "1";
                return;
            }

            double result = 1;
            for (int i = 2; i <= value; i++)
            {
                result *= i;
            }

            txtDisplay.Text = result.ToString(CultureInfo.InvariantCulture);
            newOperation = true;
        }
    }
}