using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


//using dictionaries i can probably sort the dumbSort

namespace StoryWriteHelperLite
{
    public partial class MainForm : Form
    {
        List<TextBox> Names = new List<TextBox>();
        List<TextBox> Contrls = new List<TextBox>();

        public MainForm()
        {
            InitializeComponent();            
            addFields();
        }

        
        

        

        string relativePathToFiles = "GameFiles/";
        string gameFolder; //see setFilesFolder()

        private void setFilesFolder()
        {
            gameFolder = relativePathToFiles;
        }

        string buttonsFile = "buttons.list";
        string textsFile = "texts.list";
        string controlsFile = "buttonControls.list";



        

        


        private void btn_clear_Click(object sender, EventArgs e)
        {
            File.WriteAllText(@textsFile, "");
            File.WriteAllText(@buttonsFile, "");
            File.WriteAllText(@controlsFile, "");
            txt_log.Text += "Files cleared\n";
        }

        private void clearForms()
        {            
            txt_flavor.Text = "";
            box_pageNumber.Text = "";
            box_buttonControl1.Text = "";
            box_buttonControl2.Text = "";
            box_buttonControl3.Text = "";
            box_buttonControl4.Text = "";
            box_buttonName1.Text = "";
            box_buttonName2.Text = "";
            box_buttonName3.Text = "";
            box_buttonName4.Text = "";
            isOK = true;
            fuckUp = false;
            txt_log.Text += "Form cleared\n";
        }

        private void btn_clearforms_Click(object sender, EventArgs e)
        {
            clearForms();
        }

        private void addFields()
        {
            //List<TextBox> Names = new List<TextBox>();
            Names.Add(box_buttonName1);
            Names.Add(box_buttonName2);
            Names.Add(box_buttonName3);
            Names.Add(box_buttonName4);

            //List<TextBox> Controls = new List<TextBox>();
            Contrls.Add(box_buttonControl1);
            Contrls.Add(box_buttonControl2);
            Contrls.Add(box_buttonControl3);
            Contrls.Add(box_buttonControl4);        
        }

        
        /// ////////////////
        
        private string flavorUpdate()
        {
            if (flavorIsOK() & !fuckUp)
            {
                txt_log.Text += "Content updated for page " + box_pageNumber.Text + "\n";
                return "/" + box_pageNumber.Text + "*" + txt_flavor.Text + "\n";
            }
            else { return null; }
        }

        
        private bool flavorIsOK()
        {
            try
            {
                int pageNum = Int32.Parse(box_pageNumber.Text);
            }
            catch
            {
                txt_log.Text += "Page Number is not Integer fuckup\n";
                isOK = false;
            }

            if (txt_flavor.Text == "")
            {
                txt_log.Text += "Flavor Text is empty fuckup\n";
                isOK = false;
            }
            if (box_pageNumber.Text == "")
            {
                txt_log.Text += "Page Number is empty fuckup\n";
                isOK = false;
            }
            return isOK;
        }


        
        private void buttonsUpdate()
        {
            string prefix = "/" + box_pageNumber.Text + "*";
            string buttonNamesStr = "";
            string buttonControlsStr = "";
            
            for (int i = 0; i <= 3; i++)
            {
                if (Names[i].Text != "" && Contrls[i].Text != "")
                {
                    try
                    {
                        int contrlElem = Int32.Parse(Contrls[i].Text);                        
                    }
                    catch
                    {
                        txt_log.Text += "Provide INTEGER Control fuckup\n";
                        fuckUp = true;
                        break;
                    }
                    buttonNamesStr += Names[i].Text + ";";
                    buttonControlsStr += Contrls[i].Text + ";";
                }
                /*else
                {
                    txt_log.Text += "Provide BOTH Names and Control sequentially fuckup\n";
                }*/

                //txt_log.Text += Names[i].ToString() + "\n";
                //txt_log.Text += Contrls[i].ToString() + "\n";
            }
            string rdyToWriteNames = prefix + buttonNamesStr;//yourString.Substring(0,(yourString.Length - 1));
            string rdyToWriteControls = prefix + buttonControlsStr;
            rdyToWriteNames = rdyToWriteNames.Substring(0, rdyToWriteNames.Length - 1);
            rdyToWriteControls = rdyToWriteControls.Substring(0, rdyToWriteControls.Length - 1);
            if (flavorIsOK() & !fuckUp)
            {
                File.AppendAllText(@buttonsFile, rdyToWriteNames + "\n");
                File.AppendAllText(@controlsFile, rdyToWriteControls + "\n");
            }
        }

        private void textUpdate()
        {
            File.AppendAllText(@textsFile, flavorUpdate());
        }


        private void sortFiles(string fileName) //not finished 
        {
            string flavorText;//, buttonText, buttonControls;
            flavorText = File.ReadAllText(fileName);
            //buttonText = File.ReadAllText(@buttonsFile);
            //buttonControls = File.ReadAllText(@controlsFile);
            List<string> splitterFlavorUnparsed = flavorText.Split('/').ToList();
            //List<string> splitterButtonsUnparsed = buttonText.Split('/').ToList();
            //List<string> splitterControlsUnparsed = buttonControls.Split('/').ToList();

            //List<string> splitterFlavor;

            var flavorDict = new Dictionary<int, string>();
            List<int> indexList = new List<int> { };
            //List<string> flavors = new List<string> { };


            foreach (string elem in splitterFlavorUnparsed)
            {
                string[] someArray = elem.Split('*');
                int index = Int32.Parse(someArray[0]);
                string content = someArray[1];
                indexList.Add(index);
                flavorDict.Add(index, content);
                //flavors.Add(content);
            }
            indexList.Sort();
            File.WriteAllText(fileName, "");
            string stringToReturn;
            int minimal = indexList[0];
            foreach (int elem in indexList)
            {
                string textIndex = elem.ToString();
                if (elem != minimal) { stringToReturn = "/" + textIndex + "*" + flavorDict[elem]; }
                else { stringToReturn = textIndex + "*" + flavorDict[elem]; }
                File.AppendAllText(fileName, stringToReturn);
            }
            txt_log.Text += "File " + fileName + " sorted\n";


            ////up to there

            /*int previousNumba = 0;
            txt_log.Text += "\n";
            bool isSorted = true;

            foreach (int numba in indexList)
            {
                if (previousNumba >= numba) { txt_log.Text += "\n(!) Not in place: " + previousNumba + "\n"; isSorted = false; }
                txt_log.Text += numba + ", ";
                previousNumba = numba;
            }
            if (isSorted) { txt_log.Text += "\nFiles are sorted"; }
            txt_log.Text += "\n";*/
        }





        bool isOK = true;
        bool fuckUp = false;

        private void btn_submit_Click(object sender, EventArgs e)
        {
            txt_log.Text = "";
            textUpdate();            
            buttonsUpdate();            
        }

        private void btn_index_Click(object sender, EventArgs e)
        {            
            sortFiles(textsFile);
            sortFiles(buttonsFile);
            sortFiles(controlsFile);
        }
       
    }
}
