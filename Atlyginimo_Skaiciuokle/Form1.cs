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

        //pasleptas autoriniu langas ijungus programa
        private void checkBox_Autorines_CheckedChanged(object sender, EventArgs e)
        {
            gBoxCopyright.Visible = false;
            CheckState state = checkBoxAutorines.CheckState;

            switch (state)
            {
                case CheckState.Checked:
                {
                    tBoxCopyTax.Visible = true;
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
        
        //Atlyginimo i rankas skaiciavimas
        private void bSkaiciuotiAntPopieriaus_Click(object sender, EventArgs e)
        {
            //Istraukiami kintamieji is textboxo 
            double onPaperTbox = Convert.ToDouble(this.tBoxOnPaper.Text);
            //Istraukiami kintamieji is textboxo 
            double incomeProc, insuranceProc, pensionProc, employerTaxProc;
            GetPercentageTBox(out incomeProc, out insuranceProc, out pensionProc, out employerTaxProc);

            
            //Checkboxas papildomai pensijai
            if (checkBoxExtraH.Checked == true)
            {
                pensionProc += 2;

            }

            //Mokesciu skaiciavimai su procentais metodas
            //pajamu_mokestis = Math.Round((atlyginimas_ant_popieriaus_tbox / 100 * incomeProc), 2);
            incomeTax = recalculate(onPaperTbox, incomeProc);
            insurance = Math.Round((onPaperTbox / 100 * insuranceProc), 2);
            pension = Math.Round((onPaperTbox / 100 * pensionProc), 2);
            employerTax = Math.Round((onPaperTbox / 100 * employerTaxProc), 2);
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
        private double recalculate(double onPaperTbox, double incomeProc)
        {
            return Math.Round((onPaperTbox / 100 * incomeProc), 2);
        }

        //Autoriniu skaiciavimas: atlyginimas i rankas
        private void bSkaiciuotiAutorines_Click(object sender, EventArgs e)
        {
            //Istraukiami kintamieji is textboxo 
            double copyToHands = Convert.ToDouble(this.tBoxCopyHands.Text);
            double copyTaxProc = Convert.ToDouble(this.tBoxCopyTax.Text);
            double copyOrderProc = Convert.ToDouble(this.tBoxCopyOrder.Text);

            //Mokesciu skaiciavimai su procentais
            authorToHands = Math.Round((copyToHands - (copyToHands / 100 * copyTaxProc)), 2);
            authorAll = Math.Round((copyToHands + (copyToHands / 100 * copyTaxProc)), 2);

            //Isvedimas i labeli
            labelAuthorInHands.Text = authorToHands.ToString();
            labelAuthorAll.Text = authorAll.ToString();
        }

        //Skaiviavimas atlyginimo ant popieriaus
        private void bSkaiciuotiPopierius_Click(object sender, EventArgs e)
        {
            //Istraukiami kintamaieji atlyginimas i rankas ir autorines i rankas 
            double inHandstbox = Convert.ToDouble(this.tBoxInHands.Text);
            double copyInHands = Convert.ToDouble(this.tBoxHandsCopy.Text);

            //Kvieciamas metodas kintamuju istraukimui is proc textboxu
            double incomeProc, insuranceProc, pensionProc, employerTaxProc;
            GetPercentageTBox(out incomeProc, out insuranceProc, out pensionProc, out employerTaxProc);


            //Checkboxas papildomai pensijai
            if (checkBoxExtraP.Checked == true)
            {
                pensionProc += 2;
            }


            if (copyInHands > 0 && inHandstbox > 0)
            {
                double copyOnPaper = Math.Round(copyInHands * (100 / (100 - (incomeProc + insuranceProc + pensionProc))), 2);
                double copyIncome = Math.Round((copyOnPaper * (incomeProc / 100)), 2);
                double copyInsurance = Math.Round((copyOnPaper * (insuranceProc / 100)), 2);
                double copyPension = Math.Round((copyOnPaper * (pensionProc / 100)), 2);

                //Mokesciu skaiciavimai su procentais 
                double onPaperWage = Math.Round(inHandstbox * (100 / (100 - (incomeProc + insuranceProc + pensionProc))), 2);
                double incomeWage = Math.Round((onPaperWage * (incomeProc / 100)), 2);
                double insuranceWage = Math.Round((onPaperWage * (insuranceProc / 100)), 2);
                double pensionWage = Math.Round((onPaperWage * (pensionProc / 100)), 2);
                employerTax = Math.Round((onPaperWage * (employerTaxProc / 100)), 2);
                workPlacePrice = Math.Round((onPaperWage + employerTax), 2);

                onPaper = onPaperWage + copyOnPaper;
                incomeTax = copyIncome + incomeWage;
                insurance = insuranceWage + copyInsurance;
                pension = pensionWage + copyPension;
            }
            else if (copyInHands > 0)
            {
                onPaper = Math.Round(copyInHands * (100 / (100 - (incomeProc + insuranceProc + pensionProc))), 2);
                incomeTax = Math.Round((onPaper * (incomeProc / 100)), 2);
                insurance  = Math.Round((onPaper * (insuranceProc / 100)), 2);
                pension  = Math.Round((onPaper * (pensionProc / 100)), 2);
            }

            else if (inHandstbox > 0)
            {
                onPaper = Math.Round(inHandstbox * (100 / (100 - (incomeProc + insuranceProc + pensionProc))), 2);
                incomeTax = Math.Round((onPaper * (incomeProc / 100)), 2);
                insurance = Math.Round((onPaper * (insuranceProc / 100)), 2);
                pension = Math.Round((onPaper * (pensionProc / 100)), 2);
                employerTax = Math.Round((onPaper * (employerTaxProc / 100)), 2);
                workPlacePrice = Math.Round((onPaper + employerTax), 2); 
            }

            //Isvedimas i labeli
            labelIncomeP.Text = incomeTax.ToString();
            labelInsuraceP.Text = insurance.ToString();
            labelPesionP.Text = pension.ToString();
            labelEmployerTax .Text = employerTax.ToString();
            labelOnPaper.Text = onPaper.ToString();
            labelWorkPriceP .Text = workPlacePrice.ToString();

        }

        //Metodas kintamuju istraukimui is textboxo
        private void GetPercentageTBox(out double incomeProc, out double insuranceProc, out double pensionProc, out double employerTaxProc)
        {
            incomeProc = insuranceProc = pensionProc = employerTaxProc = 0;
            double.TryParse(tBoxIncome.Text, out incomeProc);
            double.TryParse(tBoxInsurance.Text, out insuranceProc);
            double.TryParse(tBoxPension.Text, out pensionProc);
            double.TryParse(tBoxEmploTax.Text, out employerTaxProc);
        }

        //Kontrole vesti tik skaicius ir ","
        private void tBoxIncome_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxIncome.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti tik skaičius.");
                tBoxIncome.Text = "0";
            }
        }

        private void tBoxInsurance_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxInsurance.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti tik skaičius.");
                tBoxInsurance.Text = "0";
            }
        }

        private void tBoxPension_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxPension.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti tik skaičius.");
                tBoxPension.Text = "0";
            }
        }

        private void tBoxEmploTax_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxEmploTax.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti tik skaičius.");
                tBoxEmploTax.Text = "0";
            }
        }

        private void tBoxCopyTax_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxCopyTax.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti tik skaičius.");
                tBoxCopyTax.Text = "0";
            }
        }

        private void tBoxCopyOrder_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxCopyOrder.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti tik skaičius.");
                tBoxCopyOrder.Text = "0";
            }
        }

        private void tBoxOnPaper_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxOnPaper.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti tik skaičius.");
                tBoxOnPaper.Text = "0";
            }
        }

        private void tBoxCopyHands_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxCopyHands.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti tik skaičius.");
                tBoxCopyHands.Text = "0";
            }
        }

        private void tBoxInHands_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxInHands.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti tik skaičius.");
                tBoxInHands.Text = "0";
            }
        }

        private void tBoxHandsCopy_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tBoxHandsCopy.Text, "[^0-9, ,]"))
            {
                MessageBox.Show("Prašome vesti tik skaičius.");
                tBoxHandsCopy.Text = "0";
            }
        }

        
    }
}
