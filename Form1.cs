using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.QrCode;
using Keys = OpenQA.Selenium.Keys;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #region test
       
        #endregion
        public static string clickgrouplist = "//*[@data-type='groups_list']/button";
        public static string checknumberofgroup = "//section[@id='joinedGroup']/div/div[@class='MdLFT04Head']/h2[@class='mdLFT04Ttl']";
        #region copytoclipboard 
       
        #endregion

        public void AppendTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            richTextBox1.AppendText("[" + DateTime.Now.ToString("hh:mm:ss") + "]  " + value + "\n");
        }//เพิ่มคำสั่ง Log แสดงข้อความในกล่องข้อความฝั่งขวามือ

       
        private void loop_specificgroup(WebDriverWait wait, ChromeDriver driver)
        {

            try
            {
              
                var clickbuttongroup = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(clickgrouplist)));
                if (clickbuttongroup.Displayed)
                {
                    clickbuttongroup.Click();
                }
                //Groups 
                var checknumberofgrouplist = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(checknumberofgroup)));
                string numbergroup = new String(checknumberofgrouplist.Text.Where(Char.IsDigit).ToArray());
                int resultgroub = Int32.Parse(numbergroup);//แปลงเป็นตัวเลข
                Console.WriteLine(checknumberofgrouplist.Text);
                Console.WriteLine(resultgroub);
                AppendTextBox("จำนวนกลุ่มทั้งหมดกลุ่มทั้งหมด :" + resultgroub);
                ChangeUI.labelname(label1, $"จำนวนกลุ่มทั้งหมด : {resultgroub}");
                List<string> listnamegroup = new List<string>();
                //*[@id="joined_group_list_body"]/li[1]
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                var newpathfolder = Path.Combine(textBox1.Text, "txt");
                var newpathimagefolder = Path.Combine(textBox1.Text, "image");
                int fileCount = 0;
                if (!Directory.Exists(newpathfolder))
                {
                    Directory.CreateDirectory(newpathfolder);
                    AppendTextBox($"กำลังสร้าง folder : {newpathfolder} ");
                }
                if (!Directory.Exists(newpathimagefolder))
                {
                    Directory.CreateDirectory(newpathimagefolder);
                    AppendTextBox($"กำลังสร้าง folder : {newpathimagefolder} ");
                }
                //ถ้าไม่มี folder ก็แค่สร้างขึ้น ตามจริงเช็คก่อนก็ได้ if Directory.Exists แต่ขี้เกียจ
               
                try
                {
                    while (fileCount < 1)
                    {
                        // หาไฟล์txt
                        string[] txtFileNames = Directory.GetFiles(newpathfolder, "*.txt");
                        foreach (string txtFilePath in txtFileNames)
                        {
                            try
                            {
                                string txtFileName = Path.GetFileName(txtFilePath); //เอาแค่ชื่อ ไม่งั้นบัคแน่
                                string oldFilePath = Path.Combine(newpathfolder, txtFileName); //อย่าใช้string ผสมกัน ต้องใช้ combine

                                string contents = File.ReadAllText(oldFilePath);
                                if (contents != null)
                                {
                                    for (int item = 1; item <= resultgroub; item++)
                                    {
                                        Clipboard_.SetText(contents);
                                        var clickgroup = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//*[@id='joined_group_list_body']/li[{item}]/div/div[2]/div/span")));
                                        js.ExecuteScript("arguments[0].scrollIntoView();", clickgroup);
                                        clickgroup.Click();
                                        var checkmemberofgroup = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//*[@id='joined_group_list_body']/li[{item}]/div/div/p[@class='mdCMN04Desc']")));
                                        var clickchart = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@id='chat_room_input_scroll']/div[@id='_chat_room_input']")));

                                        if (clickchart != null)
                                        {
                                            Thread.Sleep(100);
                                            clickchart.Click();
                                            Thread.Sleep(100);
                                            clickchart.SendKeys(Keys.Control + "v");
                                            Thread.Sleep(100);
                                            clickchart.SendKeys(Keys.Enter);
                                        }
                                        dataGridView1.BeginInvoke(new Action(() =>
                                            {
                                                dataGridView1.Rows.Add(new object[] { item.ToString(), clickgroup.Text, checkmemberofgroup.Text, contents });
                                                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
                                            }));
                                        AppendTextBox("กำลังส่งข้อความ :" + clickgroup.Text);

                                    }
                                    string newFilePath = Path.Combine(newpathimagefolder, txtFileName);
                                    AppendTextBox($"กำลังย้าย txt จาก {txtFileName} ไป {newpathimagefolder}");
                                    File.Move(oldFilePath, newFilePath);
                                }

                            }
                            catch (Exception ex)
                            {
                                AppendTextBox($"บัคตรงย้ายtxt: {ex.Message}");
                            }
                        }

                        //หารูป
                        string[] imageFileNames = Directory.GetFiles(newpathfolder, "*.*").Where(s => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".gif") || s.EndsWith(".jpg".ToUpper()) || s.EndsWith(".png".ToUpper()) || s.EndsWith(".gif".ToUpper())).ToArray();
                        foreach (string imageFilePath in imageFileNames)
                        {
                            try
                            {

                                string imageFileName = Path.GetFileName(imageFilePath);
                                string oldFilePath = Path.Combine(newpathfolder, imageFileName);
                                if (oldFilePath != null)
                                {

                                    for (int item = 1; item <= resultgroub; item++)
                                    {
                                        ChangeUI.CopyImageToClipboardInThread(Image.FromFile(oldFilePath));

                                        var clickgroup = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//*[@id='joined_group_list_body']/li[{item}]/div/div[2]/div/span")));
                                        js.ExecuteScript("arguments[0].scrollIntoView();", clickgroup);
                                        clickgroup.Click();
                                        var checkmemberofgroup = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//*[@id='joined_group_list_body']/li[{item}]/div/div/p[@class='mdCMN04Desc']")));
                                        var clickchart = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@id='chat_room_input_scroll']/div[@id='_chat_room_input']")));

                                        if (clickchart != null)
                                        {
                                            Thread.Sleep(100);
                                            clickchart.Click();
                                            Thread.Sleep(100);
                                            clickchart.SendKeys(Keys.Control + "v");
                                            Thread.Sleep(100);
                                            clickchart.SendKeys(Keys.Enter);
                                            AppendTextBox("กำลังส่งรูปลงกลุ่ม :" + clickgroup.Text);
                                        }
                                        dataGridView1.BeginInvoke(new Action(() =>
                                        {
                                            dataGridView1.Rows.Add(new object[] { item.ToString(), clickgroup.Text, checkmemberofgroup.Text, oldFilePath });
                                            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
                                        }));

                                        AppendTextBox("เลขกลุ่ม:" + item.ToString() + "ชื่อกลุ่ม :" + clickgroup.Text);
                                    }
                                }


                                string newFilePath = Path.Combine(newpathimagefolder, imageFileName);
                                AppendTextBox($"กำลังย้ายรูปจาก{imageFileName} ไป {newpathimagefolder}");
                                File.Move(oldFilePath, newFilePath);
                            }
                            catch (Exception ex)
                            {
                                AppendTextBox($"บัคตรงย้ายรูป: {ex.Message}");
                            }
                        }


                        fileCount = txtFileNames.Length + imageFileNames.Length;
                        if (fileCount < 1)
                        {
                            AppendTextBox("ไม่มีไฟล์ที่จะโยกแล้วครับ");
                            Thread.Sleep(10000);
                        }
                    }


                }

                catch (Exception ex)
                {
                    Console.WriteLine($"Error in getting group list: {ex.Message}");
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error grouplist \n{0}", ex.Message));
            }

        }
        private void addoptions(ChromeOptions options)
        {
            options.AddExtension("Line.crx");//เพิ่ม extension 
            options.AddArguments("--headless=new"); //ซ่อนchrome 
            options.AddArguments("--disable-notifications"); // to disable notification
            options.AddArguments("--disable-application-cache"); // to disable cache
            options.AddArguments(
"--disable-background-timer-throttling",
"--disable-backgrounding-occluded-windows",
"--disable-client-side-phishing-detection",
"--disable-gpu",
"--disable-popup-blocking",
"--disable-prompt-on-repost--disable-notifications",
"--use-mock-keychain",
"--allow-running-insecure-content",
"--disable-renderer-backgrounding",
"--disable-dev-shm-usage",
"--disable-infobars",
"--mute-audio",
"--disable-field-trial-config",
"--enable-features=NetworkService,NetworkServiceInProcess",
"--disable-breakpad",
"--disable-default-apps",
"--allow-pre-commit-input",
"--disable-hang-monitor",
"--disable-ipc-flooding-protection",
"--no-first-run",
"--enable-low-end-device-mode",
"--disable-site-isolation-trials",
"--unlimited-storage",
"--disable-features=IsolateOrigins,site-per-process",
"--hide-crash-restore-bubble",
"--hide-scrollbars",
"--disable-accelerated-2d-canvas",
"--disable-accelerated-mjpeg-decode",
"--disable-accelerated-video-decode",
"--disable-accelerated-video-encode",
"--disable-and-delete-previous-log",
"--disable-app-content-verification",
"--disable-angle-features",
"--disable-audio-input",
"--disable-audio-output",
"--disable-auto-reload",
"--disable-ax-menu-list",
"--disable-back-forward-cache",
"--disable-backing-store-limit",
"--disable-canvas-aa",
"--disable-cancel-all-touches",
"--disable-checker-imaging",
"--disable-chrome-tracing-computation",
"--disable-composited-antialiasing",
"--disable-crash-reporter",
"--disable-d3d11",
"--disable-databases",
"--disable-dawn-features",
"--disable-dinosaur-easter-egg",
"--disable-direct-composition",
"--disable-direct-composition-video-overlays",
"--disable-dwm-composition",
"--disable-domain-reliability",
"--disable-es3-gl-context",
"--disable-image-animation-resync",
"--disable-lcd-text",
"--disable-media-session-api",
"--disable-modal-animations",
"--disable-pinch",
"--disable-print-preview",
"--disable-pull-to-refresh-effect",
"--disable-rgba-4444-textures",
"--disable-smooth-scrolling",
"--disable-software-rasterizer",
"--disable-webgl",
"--disable-windows10-custom-titlebar",
"--disable-speech-api",
"--disable-speech-synthesis-api",
"--disable-stack-profiler",
"--disable-touch-drag-drop",
"--disable-virtual-keyboard",
"--disable-volume-adjust-sound"
);
        }
        private void loginwithqrcode(WebDriverWait wait)
        {
            try
            {
                string login_qr_btn = "//*[@id='login_qr_btn']";//ปุ่มกด qr code
                string getqrcode = "//div[@class='mdCMN01Code']";

                if (wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(login_qr_btn))).Displayed)
                {
                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(login_qr_btn))).Click();
                }//click login qrcode


                var qrcode = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='login_qrcode_area']/div[1]/div")));
                string linkforconvert = qrcode.GetAttribute("title");
                if (qrcode.Displayed)
                {

                    Console.Write(linkforconvert);//แปลงลิ้งให้เป็น qr code
                    QrCodeEncodingOptions optionsofqrcode = new QrCodeEncodingOptions
                    {
                        DisableECI = true,
                        CharacterSet = "UTF-8",
                        Width = 200,
                        Height = 200
                    };

                    BarcodeWriter writer = new BarcodeWriter();
                    writer.Format = BarcodeFormat.QR_CODE;
                    writer.Options = optionsofqrcode;
                    Bitmap bitmap = writer.Write(linkforconvert);
                    if (!Directory.Exists(@".\login"))
                    {
                        Directory.CreateDirectory(@".\login");
                    }
                    else
                    {
                        bitmap.Save(@".\login\QRCode.png", System.Drawing.Imaging.ImageFormat.Png);
                        Console.WriteLine("QR Code generated successfully.");

                        pictureBox1.ImageLocation = @".\login\QRCode.png";//ดึงรูปภาพจากไฟล์
                        AppendTextBox("กรุณาสแกน QR CODE ภายใน 2 นาที");
                    }

                }

                if (wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(getqrcode))).Displayed)
                {
                    var qrcodeTEXT = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(getqrcode))).Text;
                    ChangeUI.labelname(label2, $"OTP LOGIN : {qrcodeTEXT}");
                    label2.ForeColor = Color.Green;
                    Console.WriteLine(qrcodeTEXT);
                    AppendTextBox($"รหัส OTP คือ {qrcodeTEXT}");

                }//ดึงรหัส otp 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : Page_login_qr_code\t:" + ex.Message);
            }//
        }
        private void  main()
        {
            ChromeOptions options = new ChromeOptions();
            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            addoptions(options);
            driverService.HideCommandPromptWindow = true;
            var processidchrom = driverService.ProcessId;
            Console.WriteLine(processidchrom);
            ChromeDriver driver = new ChromeDriver(driverService, options);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            driver.Navigate().GoToUrl("chrome-extension://ophjlpahpchlmihnnnihgmmeilfjmjjc/index.html");//ไปที่เว็บ
            AppendTextBox("โปรแกรมกำลังทำงาน โดยการใช้งานแบบส่งรายบุคคล โปรดรอสักครู่...");
            MainFunction.Login_QrCode(wait,label2,pictureBox1);

            loop_specificgroup(wait, driver);
        }
        private void checkseleniume_Exist()
        {
            try
            {
                foreach (var process in Process.GetProcessesByName("chromedriver"))
                {
                    Console.WriteLine(process);
                    AppendTextBox($"ChromeDriver : {process}still exist");
                    process.Kill();
                }
                foreach (var process in Process.GetProcessesByName("chrome"))
                {
                    process.Kill();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!backgroundWorker1.CancellationPending)
            {
                testloop();
                //main();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            backgroundWorker1.RunWorkerAsync();
        }
        private void testloop()
        {
            ChromeOptions options = new ChromeOptions();
            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            addoptions(options);
            driverService.HideCommandPromptWindow = true;
            
            ChromeDriver driver = new ChromeDriver(driverService, options);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            driver.Navigate().GoToUrl("chrome-extension://ophjlpahpchlmihnnnihgmmeilfjmjjc/index.html");//ไปที่เว็บ
            AppendTextBox("โปรแกรมกำลังทำงาน โดยการใช้งานแบบส่งรายบุคคล โปรดรอสักครู่...");
            MainFunction.Login_QrCode(wait, label2, pictureBox1);

          
            MainFunction.Wait_and_Click(driver, By.XPath(clickgrouplist), 20, true);
            var checknumberofgrouplist = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(checknumberofgroup)));
            string numbergroup = new String(checknumberofgrouplist.Text.Where(Char.IsDigit).ToArray());
            int resultgroub = Int32.Parse(numbergroup);//แปลงเป็นตัวเลข
            Console.WriteLine(checknumberofgrouplist.Text);
            Console.WriteLine(resultgroub);
            AppendTextBox("จำนวนกลุ่มทั้งหมดกลุ่มทั้งหมด :" + resultgroub);
            ChangeUI.labelname(label1, $"จำนวนกลุ่มทั้งหมด : {resultgroub}");
            List<string> listnamegroup = new List<string>();
            //*[@id="joined_group_list_body"]/li[1]
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            var newpathfolder = Path.Combine(textBox1.Text, "txt");
            var newpathimagefolder = Path.Combine(textBox1.Text, "image");
            if (!Directory.Exists(newpathfolder))
            {
                Directory.CreateDirectory(newpathfolder);
                AppendTextBox($"กำลังสร้าง folder : {newpathfolder} ");
            }
            if (!Directory.Exists(newpathimagefolder))
            {
                Directory.CreateDirectory(newpathimagefolder);
                AppendTextBox($"กำลังสร้าง folder : {newpathimagefolder} ");
            }
            
            int lenfileIMAG = Directory.GetFiles(newpathimagefolder, "*.*").Where(s => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".gif") || s.EndsWith(".jpg".ToUpper()) || s.EndsWith(".png".ToUpper()) || s.EndsWith(".gif".ToUpper())).ToArray().Length;
            int lenfileTXT = Directory.GetFiles(newpathfolder, "*.txt").Length;

            Console.WriteLine($"Number of file txt {lenfileTXT.ToString()}");
            Console.WriteLine($"Number of file image {lenfileIMAG.ToString()}");
            List<string> list_txt = new List<string>();
            List<string> list_img = new List<string>();

            while (true)
            {
                while(lenfileIMAG > 0 || lenfileTXT >0)
                {
                    AppendTextBox($"มีไฟล์เข้ามา ข้อความ : {lenfileTXT} รูปภาพ : {lenfileIMAG}");
                    if (lenfileTXT > 0 || lenfileIMAG > 0)
                    {

                        if (lenfileTXT > 0)
                        {
                            string[] txtFileNames = Directory.GetFiles(newpathfolder, "*.txt");
                            foreach (var txtfile in txtFileNames)
                            {
                                if (File.ReadAllText(Path.Combine(newpathfolder, txtfile)) != string.Empty)
                                {
                                    AppendTextBox($"ชื่อไฟล์ txt : {txtfile.ToString()}");
                                    var contentintxt = File.ReadAllText(Path.Combine(newpathfolder, txtfile));
                                    list_txt.Add(contentintxt);
                                    File.Delete(Path.Combine(newpathfolder, txtfile));
                                    AppendTextBox($"ลบไฟล์ txt : {txtfile.ToString()}");
                                }
                                else
                                {
                                    AppendTextBox($"ชื่อไฟล์ txt : {txtfile.ToString()} ไม่มีข้อความอยู่");
                                }


                            }
                        }
                        if (lenfileIMAG > 0)
                        {
                            string[] imageFileNames = Directory.GetFiles(newpathimagefolder, "*.*").Where(s => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".gif") || s.EndsWith(".jpg".ToUpper()) || s.EndsWith(".png".ToUpper()) || s.EndsWith(".gif".ToUpper())).ToArray();
                            foreach (var image_file in imageFileNames)
                            {
                                var pathcombine_img = Path.Combine(newpathimagefolder, image_file);
                                AppendTextBox($"ที่อยู่ไฟล์ image : {pathcombine_img.ToString()}");
                                list_img.Add(pathcombine_img);
                               
                            }
                        }

                        for (int item = 1; item <= resultgroub; item++)
                        {
                            var clickgroup = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//*[@id='joined_group_list_body']/li[{item}]/div/div[2]/div/span")));
                            js.ExecuteScript("arguments[0].scrollIntoView();", clickgroup);
                            js.ExecuteScript("window.scrollBy(0, -window.innerHeight / 2);");
                            clickgroup.Click();
                            var checkmemberofgroup = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//*[@id='joined_group_list_body']/li[{item}]/div/div/p[@class='mdCMN04Desc']")));
                            var clickchart = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@id='chat_room_input_scroll']/div[@id='_chat_room_input']")));

                            if (clickchart != null)
                            {

                                if (list_txt != null && list_txt.Count > 0)
                                {
                                    AppendTextBox("กำลังส่งข้อความ :" + clickgroup.Text);
                                    foreach (var copytxt in list_txt)
                                    {
                                        AppendTextBox(copytxt);
                                        Clipboard_.SetText(copytxt);
                                        clickchart.SendKeys(Keys.Control + "v");
                                        Thread.Sleep(1000);
                                        clickchart.SendKeys(Keys.Enter);
                                    }
                                }
                                
                                if (list_img != null && list_img.Count > 0)
                                {
                                    AppendTextBox("กำลังส่งรูป :" + clickgroup.Text);
                                    foreach (var copy_image in list_img)
                                    {
                                        AppendTextBox(copy_image);
                                        ChangeUI.CopyImageToClipboardInThread(Image.FromFile(Path.Combine(copy_image)));
                                        clickchart.SendKeys(Keys.Control + "v");
                                        Thread.Sleep(1000);
                                        clickchart.SendKeys(Keys.Enter);
                                    }
                                }

                            }
                        }
                        js.ExecuteScript("window.scrollTo(0, 40);");
                        list_img.Clear();
                        list_txt.Clear();



                    }

                    lenfileIMAG = Directory.GetFiles(newpathfolder, "*.*").Where(s => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".gif") || s.EndsWith(".jpg".ToUpper()) || s.EndsWith(".png".ToUpper()) || s.EndsWith(".gif".ToUpper())).ToArray().Length;
                    lenfileTXT = Directory.GetFiles(newpathfolder, "*.txt").Length;
                    AppendTextBox($"ไฟล์ทั้งหมดตอนนี้ ข้อความ : {lenfileTXT} รูปภาพ : {lenfileIMAG}");
                }

                while(lenfileIMAG ==0  && lenfileTXT == 0)
                {
                   
                    Thread.Sleep(60000);
                    lenfileIMAG = Directory.GetFiles(newpathimagefolder, "*.*").Where(s => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".gif") || s.EndsWith(".jpg".ToUpper()) || s.EndsWith(".png".ToUpper()) || s.EndsWith(".gif".ToUpper())).ToArray().Length;
                    lenfileTXT = Directory.GetFiles(newpathfolder, "*.txt").Length;
                }
                
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            checkseleniume_Exist();
            
            
        }
    }
}
