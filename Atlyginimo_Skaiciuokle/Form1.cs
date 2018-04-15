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
        public double atlyginimas_i_rankas;
        public double atlyginimas_ant_popieriaus;
        public double pajamu_mokestis;
        public double sveikatos_draudimas;
        public double pensiju_draudimas;
        public double darbdavio_mokesciai;
        public double darbo_vietos_kaina;
        public double autorines_i_rankas;
        public double autorines_uzsakovo_suma;
        

        public Atlyginimo_Skaiciuokle()
        {
            InitializeComponent();
        }

        //pasleptas autoriniu langas ijungus programa 
        private void checkBox_Autorines_CheckedChanged(object sender, EventArgs e)
        {
            gBoxAutorines.Visible = false;
            CheckState state = checkBoxAutorines.CheckState;

            switch (state)
            {
                case CheckState.Checked:
                {
                    tBoxCopyTax.Visible = true;
                    gBoxAutorines.Visible = true;
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
            double atlyginimas_ant_popieriaus_tbox = 0;
            //Istraukiamas kintamasis atlyginimas ant popieriaus is textboxo 
            if (IsNumber(tBoxOnPaper.Text))
            {
                atlyginimas_ant_popieriaus_tbox = Convert.ToDouble(this.tBoxOnPaper.Text);
            }
            else
            {
                MessageBox.Show("Turite įvesti skaičių");
                return;
            }

            //Istraukiami kintamieji is textboxo 
            double pajamu_mokestis_tbox, sveikatos_draudimas_tbox, pensiju_draudimas_tbox, darbdavio_mokesciai_tbox;
            IstraukimasIsTBox(out pajamu_mokestis_tbox, out sveikatos_draudimas_tbox, out pensiju_draudimas_tbox, out darbdavio_mokesciai_tbox);

            
            //Checkboxas papildomai pensijai
            if (checkBoxExtraH.Checked == true)
            {
                pensiju_draudimas_tbox += 2;

            }

            //Mokesciu skaiciavimai su procentais metodas
            pajamu_mokestis = Math.Round((atlyginimas_ant_popieriaus_tbox / 100 * pajamu_mokestis_tbox), 2);
            //pajamu_mokestis = recalculate(atlyginimas_ant_popieriaus, pajamu_mokestis_tbox);
            sveikatos_draudimas = Math.Round((atlyginimas_ant_popieriaus_tbox / 100 * sveikatos_draudimas_tbox), 2);
            pensiju_draudimas = Math.Round((atlyginimas_ant_popieriaus_tbox / 100 * pensiju_draudimas_tbox), 2);
            darbdavio_mokesciai = Math.Round((atlyginimas_ant_popieriaus_tbox / 100 * darbdavio_mokesciai_tbox), 2);
            atlyginimas_i_rankas = Math.Round((atlyginimas_ant_popieriaus_tbox - pajamu_mokestis - sveikatos_draudimas - pensiju_draudimas), 2);
            darbo_vietos_kaina = Math.Round((atlyginimas_ant_popieriaus_tbox + darbdavio_mokesciai), 2);

            //Isvedimas i labeli
            labelIncome.Text = pajamu_mokestis.ToString();
            labelInsurance.Text = sveikatos_draudimas.ToString();
            labelPension.Text = pensiju_draudimas.ToString();
            labelEmplTax.Text = darbdavio_mokesciai.ToString();
            labelInHands.Text = atlyginimas_i_rankas.ToString();
            labelWorkPrice.Text = darbo_vietos_kaina.ToString();
        }
        private double recalculate(double atlyginimas_ant_popieriaus, double pajamu_mokestis_tbox)
        {
            return Math.Round((atlyginimas_ant_popieriaus / 100 * pajamu_mokestis_tbox), 2);
        }

        //Autoriniu skaiciavimas: atlyginimas i rankas
        private void bSkaiciuotiAutorines_Click(object sender, EventArgs e)
        {
            //Istraukiami kintamieji is textboxo 
            double autorines_pajamos_tbox = Convert.ToDouble(this.tBoxCopyHands.Text);
            double autoriniai_mokesciai_tbox = Convert.ToDouble(this.tBoxCopyTax.Text);
            double autoriniai_uzsakovo_tbox = Convert.ToDouble(this.tBoxCopyOrder.Text);

            //Mokesciu skaiciavimai su procentais
            autorines_i_rankas = Math.Round((autorines_pajamos_tbox - (autorines_pajamos_tbox / 100 * autoriniai_mokesciai_tbox)), 2);
            autorines_uzsakovo_suma = Math.Round((autorines_pajamos_tbox + (autorines_pajamos_tbox / 100 * autoriniai_mokesciai_tbox)), 2);

            //Isvedimas i labeli
            labelAutorinesRankas.Text = autorines_i_rankas.ToString();
            labelAutorinesUzsakSuma.Text = autorines_uzsakovo_suma.ToString();
        }

        //Skaiviavimas atlyginimo ant popieriaus
        private void bSkaiciuotiPopierius_Click(object sender, EventArgs e)
        {
            //Istraukiami kintamaieji atlyginimas i rankas ir autorines i rankas 
            double atlyginimas_i_rankas_tbox = Convert.ToDouble(this.tBoxInHands.Text);
            double autorines_i_rankas_tbox = Convert.ToDouble(this.tBoxHandsCopy.Text);

            //Kvieciamas metodas kintamuju istraukimui is proc textboxu
            double pajamu_mokestis_tbox, sveikatos_draudimas_tbox, pensiju_draudimas_tbox, darbdavio_mokesciai_tbox;
            IstraukimasIsTBox(out pajamu_mokestis_tbox, out sveikatos_draudimas_tbox, out pensiju_draudimas_tbox, out darbdavio_mokesciai_tbox);

            //Checkboxas papildomai pensijai
            if (checkBoxExtraP.Checked == true)
            {
                pensiju_draudimas_tbox += 2;
            }


            if (autorines_i_rankas_tbox > 0 && atlyginimas_i_rankas_tbox > 0)
            {
                double autorines_popierius = Math.Round(autorines_i_rankas_tbox * (100 / (100 - (pajamu_mokestis_tbox + sveikatos_draudimas_tbox + pensiju_draudimas_tbox))), 2);
                double pajamu_autorines = Math.Round((autorines_popierius * (pajamu_mokestis_tbox / 100)), 2);
                double sveikatos_autorines = Math.Round((autorines_popierius * (sveikatos_draudimas_tbox / 100)), 2);
                double pensiju_autorines = Math.Round((autorines_popierius * (pensiju_draudimas_tbox / 100)), 2);

                //Mokesciu skaiciavimai su procentais 
                double atlyginimas_ant_popieriaus2 = Math.Round(atlyginimas_i_rankas_tbox * (100 / (100 - (pajamu_mokestis_tbox + sveikatos_draudimas_tbox + pensiju_draudimas_tbox))), 2);
                double pajamu_mokestis2 = Math.Round((atlyginimas_ant_popieriaus2 * (pajamu_mokestis_tbox / 100)), 2);
                double sveikatos_draudimas2 = Math.Round((atlyginimas_ant_popieriaus2 * (sveikatos_draudimas_tbox / 100)), 2);
                double pensiju_draudimas2 = Math.Round((atlyginimas_ant_popieriaus2 * (pensiju_draudimas_tbox / 100)), 2);
                darbdavio_mokesciai = Math.Round((atlyginimas_ant_popieriaus2 * (darbdavio_mokesciai_tbox / 100)), 2);
                darbo_vietos_kaina = Math.Round((atlyginimas_ant_popieriaus2 + darbdavio_mokesciai), 2);

                atlyginimas_ant_popieriaus = atlyginimas_ant_popieriaus2 + autorines_popierius;
                pajamu_mokestis = pajamu_autorines + pajamu_mokestis2;
                sveikatos_draudimas = sveikatos_draudimas2 + sveikatos_autorines;
                pensiju_draudimas = pensiju_draudimas2 + pensiju_autorines;
            }
            else if (autorines_i_rankas_tbox > 0)
            {
                atlyginimas_ant_popieriaus = Math.Round(autorines_i_rankas_tbox * (100 / (100 - (pajamu_mokestis_tbox + sveikatos_draudimas_tbox + pensiju_draudimas_tbox))), 2);
                pajamu_mokestis = Math.Round((atlyginimas_ant_popieriaus * (pajamu_mokestis_tbox / 100)), 2);
                sveikatos_draudimas  = Math.Round((atlyginimas_ant_popieriaus * (sveikatos_draudimas_tbox / 100)), 2);
                pensiju_draudimas  = Math.Round((atlyginimas_ant_popieriaus * (pensiju_draudimas_tbox / 100)), 2);
            }

            else if (atlyginimas_i_rankas_tbox > 0)
            {
                atlyginimas_ant_popieriaus = Math.Round(atlyginimas_i_rankas_tbox * (100 / (100 - (pajamu_mokestis_tbox + sveikatos_draudimas_tbox + pensiju_draudimas_tbox))), 2);
                pajamu_mokestis = Math.Round((atlyginimas_ant_popieriaus * (pajamu_mokestis_tbox / 100)), 2);
                sveikatos_draudimas = Math.Round((atlyginimas_ant_popieriaus * (sveikatos_draudimas_tbox / 100)), 2);
                pensiju_draudimas = Math.Round((atlyginimas_ant_popieriaus * (pensiju_draudimas_tbox / 100)), 2);
                darbdavio_mokesciai = Math.Round((atlyginimas_ant_popieriaus * (darbdavio_mokesciai_tbox / 100)), 2);
                darbo_vietos_kaina = Math.Round((atlyginimas_ant_popieriaus + darbdavio_mokesciai), 2); 
            }

            //Isvedimas i labeli
            labelIncomeP.Text = pajamu_mokestis.ToString();
            labelInsuraceP.Text = sveikatos_draudimas.ToString();
            labelPesionP.Text = pensiju_draudimas.ToString();
            labelEmployerTax .Text = darbdavio_mokesciai.ToString();
            labelOnPaper.Text = atlyginimas_ant_popieriaus.ToString();
            labelWorkPriceP .Text = darbo_vietos_kaina.ToString();

        }

        //Metodas kintamuju istraukimui is textboxo
        private void IstraukimasIsTBox(out double pajamu_mokestis_tbox, out double sveikatos_draudimas_tbox, out double pensiju_draudimas_tbox, out double darbdavio_mokesciai_tbox)
        {
            pajamu_mokestis_tbox = Convert.ToDouble(this.tBoxIncome.Text);
            sveikatos_draudimas_tbox = Convert.ToDouble(this.tBoxInsurance.Text);
            pensiju_draudimas_tbox = Convert.ToDouble(this.tBoxPension.Text);
            darbdavio_mokesciai_tbox = Convert.ToDouble(this.tBoxEmploTax.Text);
        }

        private bool IsNumber(string input)
        {
            int n;
            bool isNumber = false;
            isNumber = int.TryParse(input, out n);

            if (n <= 0)
            {
                isNumber = false;

            }
            return isNumber;
        }       
    }
}
