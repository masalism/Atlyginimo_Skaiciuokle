using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atlyginimo_Skaiciuokle
{
    public partial class Atlyginimo_Skaiciuokle : Form
    {
        //kintamieji
        public double inHands;
        public double onPaper;
        public double incomeTax;
        public double insurance;
        public double pension;
        public double employerTax;
        public double workPlacePrice;
        public double authorToHands;
        public double authorAll;
        
        public Atlyginimo_Skaiciuokle()
        {
            InitializeComponent();
        }
        
        //Atlyginimo i rankas skaiciavimas
        private void bCountToHands_Click(object sender, EventArgs e)
        {
            //Istraukiami kintamieji is textboxo, jei tuscia = 0;
            double onPaperTbox = 0;
            double.TryParse(tBoxOnPaper.Text, out onPaperTbox);
            double incomeProc, insuranceProc, pensionProc, employerTaxProc;
            GetPercentageTBox(out incomeProc, out insuranceProc, out pensionProc, out employerTaxProc);

            //Checkboxas papildomai pensijai
            if (checkBoxExtraH.Checked == true)
            {
                pensionProc += 2;
            }

            //Mokesciu skaiciavimai 
            incomeTax = CalcTaxInHands(onPaperTbox, incomeProc);
            insurance = CalcTaxInHands(onPaperTbox, insuranceProc);
            pension = CalcTaxInHands(onPaperTbox, pensionProc);
            employerTax = CalcTaxInHands(onPaperTbox, employerTaxProc);
            inHands = Math.Round((onPaperTbox - incomeTax - insurance - pension), 2);
            workPlacePrice = Math.Round((onPaperTbox + employerTax), 2);

            //Isvedimas i labeli
            labelIncome.Text = incomeTax.ToString();
            labelInsurance.Text = insurance.ToString();
            labelPension.Text = pension.ToString();
            labelEmplTax.Text = employerTax.ToString();
            labelInHands.Text = inHands.ToString();
            labelWorkPrice.Text = workPlacePrice.ToString();
        }

        //autoriniu sutarciu skaiciavimas
        private void bCountAuthor_Click(object sender, EventArgs e)
        {
            double copyToHands = 0;
            double copyTaxProc = 0;
            double copyOrderProc = 0;
            double.TryParse(tBoxCopyHands.Text, out copyToHands);
            double.TryParse(tBoxCopyTax.Text, out copyTaxProc);
            double.TryParse(tBoxCopyOrder.Text, out copyOrderProc);

            //Mokesciu skaiciavimai
            authorToHands = Math.Round((copyToHands - (copyToHands / 100 * copyTaxProc)), 2);
            authorAll = Math.Round((copyToHands + (copyToHands / 100 * copyTaxProc)), 2);

            //Isvedimas i labeli
            labelAuthorInHands.Text = authorToHands.ToString();
            labelAuthorAll.Text = authorAll.ToString();
        }

        //Atlyginimo ant popieriaus skaicaivimas
        private void bCountOnPaper_Click(object sender, EventArgs e)
        {
            //Istraukiami kintamaieji is textboxu, jei tusti = 0;
            double inHandsTBox = 0;
            double copyInHands = 0;
            double.TryParse(tBoxInHands.Text, out inHandsTBox);
            double.TryParse(tBoxHandsCopy.Text, out copyInHands);
            double incomeProc, insuranceProc, pensionProc, employerTaxProc;
            GetPercentageTBox(out incomeProc, out insuranceProc, out pensionProc, out employerTaxProc);

            //Checkboxas papildomai pensijai
            if (checkBoxExtraP.Checked == true)
            {
                pensionProc += 2;
            }

            //Mokesciu skaiciavimai ant popieriaus su autorinemis teisemis
            if (copyInHands > 0 && inHandsTBox > 0)
            {
                double copyOnPaper = CalcOnPaper(copyInHands, incomeProc, insuranceProc, pensionProc);
                double copyIncome = CalcTaxOnPaper(copyOnPaper, incomeProc);
                double copyInsurance = CalcTaxOnPaper(copyOnPaper, insuranceProc);
                double copyPension = CalcTaxOnPaper(copyOnPaper, pensionProc);

                double onPaperWage = CalcOnPaper(inHandsTBox, incomeProc, insuranceProc, pensionProc);
                double incomeWage = CalcTaxOnPaper(onPaperWage, incomeProc);
                double insuranceWage = CalcTaxOnPaper(onPaperWage, insuranceProc);
                double pensionWage = CalcTaxOnPaper(onPaperWage, pensionProc);
                employerTax = CalcTaxOnPaper(onPaperWage, employerTaxProc);
                workPlacePrice = Math.Round((onPaperWage + employerTax), 2);

                onPaper = onPaperWage + copyOnPaper;
                incomeTax = copyIncome + incomeWage;
                insurance = insuranceWage + copyInsurance;
                pension = pensionWage + copyPension;
            }

            //tik autoriniu sutarciu skaiciavimai
            else if (copyInHands > 0)
            {
                onPaper = CalcOnPaper(copyInHands, incomeProc, insuranceProc, pensionProc);
                incomeTax = CalcTaxOnPaper(onPaper, incomeProc);
                insurance = CalcTaxOnPaper(onPaper, insuranceProc);
                pension = CalcTaxOnPaper(onPaper, pensionProc);
                employerTax = 0;
                workPlacePrice = 0;
            }

            //tik ant popieriaus skaiciavimai
            else if (inHandsTBox > 0)
            {
                onPaper = CalcOnPaper(inHandsTBox, incomeProc, insuranceProc, pensionProc);
                incomeTax = CalcTaxOnPaper(onPaper, incomeProc);
                insurance = CalcTaxOnPaper(onPaper, insuranceProc);
                pension = CalcTaxOnPaper(onPaper, pensionProc);
                employerTax = CalcTaxOnPaper(onPaper, employerTaxProc);
                workPlacePrice = Math.Round((onPaper + employerTax), 2);
            }
            else
            {
                onPaper = incomeTax = insurance = employerTax = pension = workPlacePrice = 0;
            }

            //Isvedimas i labeli
            labelIncomeP.Text = incomeTax.ToString();
            labelInsuraceP.Text = insurance.ToString();
            labelPesionP.Text = pension.ToString();
            labelEmployerTax.Text = employerTax.ToString();
            labelOnPaper.Text = onPaper.ToString();
            labelWorkPriceP.Text = workPlacePrice.ToString();
        }

        //Metodas vienodu kintamuju istraukimui is textboxo
        private void GetPercentageTBox(out double incomeProc, out double insuranceProc, out double pensionProc, out double employerTaxProc)
        {
            //visus prisilyginam nuliui jei textboxas butu tuscias
            incomeProc = insuranceProc = pensionProc = employerTaxProc = 0;
            double.TryParse(tBoxIncome.Text, out incomeProc);
            double.TryParse(tBoxInsurance.Text, out insuranceProc);
            double.TryParse(tBoxPension.Text, out pensionProc);
            double.TryParse(tBoxEmploTax.Text, out employerTaxProc);
        }

        //Skaiciavimo metodai vienodiems veiksmams
        private double CalcTaxInHands(double num1, double num2)
        {
            return Math.Round((num1 / 100 * num2), 2);
        }
        private double CalcTaxOnPaper(double num1, double num2)
        {
            return Math.Round((num1 * (num2 / 100)), 2);
        }
        private double CalcOnPaper(double num1, double num2, double num3, double num4)
        {
            return Math.Round(num1 * (100 / (100 - (num2 + num3 + num4))), 2);
        }

        //pasleptas autoriniu sutarciu langas ijungus programa
        private void checkBoxCopyright_CheckedChanged(object sender, EventArgs e)
        {
            gBoxCopyright.Visible = false;
            CheckState state = checkBoxCopyright.CheckState;

            switch (state)
            {
                case CheckState.Checked:
                {
                    gBoxCopyright.Visible = true;
                    break;
                }
                case CheckState.Indeterminate:
                case CheckState.Unchecked:
                {
                    break;
                }
            }
        }

        //Isvalymo mygtukas
        private void bClear_Click(object sender, EventArgs e)
        {
            this.tBoxIncome.Text = "0";
            this.tBoxInsurance.Text = "0";
            this.tBoxPension.Text = "0";
            this.tBoxCopyTax.Text = "0";
            this.tBoxEmploTax.Text = "0";
            this.tBoxCopyOrder.Text = "0";
        }

        //Kontrole vesti tik skaicius ir ","
        private void tBoxIncome_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxIncome.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti skaičius.");
                tBoxIncome.Text = "0";
            }
        }

        private void tBoxInsurance_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxInsurance.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti skaičius.");
                tBoxInsurance.Text = "0";
            }
        }

        private void tBoxPension_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxPension.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti skaičius.");
                tBoxPension.Text = "0";
            }
        }

        private void tBoxEmploTax_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxEmploTax.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti skaičius.");
                tBoxEmploTax.Text = "0";
            }
        }

        private void tBoxCopyTax_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxCopyTax.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti skaičius.");
                tBoxCopyTax.Text = "0";
            }
        }

        private void tBoxCopyOrder_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxCopyOrder.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti skaičius.");
                tBoxCopyOrder.Text = "0";
            }
        }

        private void tBoxOnPaper_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxOnPaper.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti skaičius.");
                tBoxOnPaper.Text = "0";
            }
        }

        private void tBoxCopyHands_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxCopyHands.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti skaičius.");
                tBoxCopyHands.Text = "0";
            }
        }

        private void tBoxInHands_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxInHands.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti skaičius.");
                tBoxInHands.Text = "0";
            }
        }

        private void tBoxHandsCopy_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxHandsCopy.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti skaičius.");
                tBoxHandsCopy.Text = "0";
            }
        }
    } 
}
