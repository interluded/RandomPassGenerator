using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandomPassGenerator
{
    public partial class Form1 : Form
    {
        string resultClass;
        public Form1()
        {
            InitializeComponent();
            button1.Text = "Generate";
            label1.Text = "click generate";
            label2.Text = "# of chars";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string randomChars = await GenerateRandomChars();
            MessageBox.Show(randomChars);
            label1.Text = resultClass;
            Clipboard.SetText(randomChars); 
            MessageBox.Show("Password copied to clipboard.");
            await Task.Delay(3000);
            label1.Text = "********************";
        }

        public async Task<string> GenerateRandomChars()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"https://www.random.org/integers/?num={textBox1.Text}&min=48&max=122&col=1&base=10&format=plain&rnd=new");

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        string[] numbers = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                        string result = "";
                        foreach (var number in numbers)
                        {
                            if (int.TryParse(number, out int randomNumber))
                            {
                                char randomChar = (char)randomNumber;
                                result += randomChar;
                                resultClass = result; // Store the result
                            }
                            else
                            {
                                return $"error: not able to parse '{number}' as an integer.";
                            }
                        }

                        return result;
                    }
                    else
                    {
                        return "Error: " + response.StatusCode;
                    }
                }
                catch (HttpRequestException ex)
                {
                    return "Error: " + ex.Message;
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
