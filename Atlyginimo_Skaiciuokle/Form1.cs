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
            //Istraukiami kintamieji is textboxo 
            double atlyginimas_ant_popieriaus = Convert.ToDouble(this.tBoxAtlyginimasAntPopieriaus.Text);
            double pajamu_mokestis_tbox = Convert.ToDouble(this.tBoxPajamuProc.Text);
            double sveikatos_draudimas_tbox = Convert.ToDouble(this.tBoxSveikatosDraudimasProc.Text);
            double pensiju_draudimas_tbox = Convert.ToDouble(this.tBoxPensijuDraudimasProc.Text);
            double darbdavio_mokesciai_tbox = Convert.ToDouble(this.tBoxDarbdavioMokesciaiProc.Text);

            //Checkboxas papildomai pensijai
            if (checkBoxPapildomaPensija.Checked == true)
            {
                pensiju_draudimas_tbox += 2;

            }
            //Mokesciu skaiciavimai su procentais
            pajamu_mokestis = Math.Round((atlyginimas_ant_popieriaus / 100 * pajamu_mokestis_tbox), 2);
            sveikatos_draudimas = Math.Round((atlyginimas_ant_popieriaus / 100 * sveikatos_draudimas_tbox), 2);
            pensiju_draudimas = Math.Round((atlyginimas_ant_popieriaus / 100 * pensiju_draudimas_tbox), 2);
            darbdavio_mokesciai = Math.Round((atlyginimas_ant_popieriaus / 100 * darbdavio_mokesciai_tbox), 2);
            atlyginimas_i_rankas = Math.Round((atlyginimas_ant_popieriaus - pajamu_mokestis - sveikatos_draudimas - pensiju_draudimas), 2);
            darbo_vietos_kaina = Math.Round((atlyginimas_ant_popieriaus + darbdavio_mokesciai), 2);

            //isvedimas i labeli
            labelPajamuMokestis.Text = pajamu_mokestis.ToString();
            labelSveikatosDraudimas.Text = sveikatos_draudimas.ToString();
            labelPensijuDraudimas.Text = pensiju_draudimas.ToString();
            labelDarbdavioMokesciaiSodrai.Text = darbdavio_mokesciai.ToString();
            labelAtlyginimasIRankas.Text = atlyginimas_i_rankas.ToString();
            labelDarboVietosKaina.Text = darbo_vietos_kaina.ToString();          
        }

        //autoriniu skaiciavimas: atlyginimas i rankas
        private void bSkaiciuotiAutorines_Click(object sender, EventArgs e)
        {
            //Istraukiami kintamieji is textboxo 
            double autorines_pajamos_tbox = Convert.ToDouble(this.tBoxPajamosAutorines.Text);
            double autoriniai_mokesciai_tbox = Convert.ToDouble(this.tBoxAutoriniaiMokesciaiProc.Text);
            double autoriniai_uzsakovo_tbox = Convert.ToDouble(this.tBoxUzsakovoMokesciaiProc.Text);

            //Mokesciu skaiciavimai su procentais
            autorines_i_rankas = Math.Round((autorines_pajamos_tbox - (autorines_pajamos_tbox / 100 * autoriniai_mokesciai_tbox)), 2);
            autorines_uzsakovo_suma = Math.Round((autorines_pajamos_tbox + (autorines_pajamos_tbox / 100 * autoriniai_mokesciai_tbox)), 2);

            //isvedimas i labeli
            labelAutorinesRankas.Text = autorines_i_rankas.ToString();
            labelAutorinesUzsakSuma.Text = autorines_uzsakovo_suma.ToString();
        }
    }
}
