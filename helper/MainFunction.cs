using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using System.Windows.Forms;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using ZXing.QrCode;
using ZXing;
using System.Net;
using System.Threading;

namespace WindowsFormsApp1
{
    internal class MainFunction
    {
        public static void ADD_optionsExtenandArg(ChromeOptions options)
        {
            options.AddExtension("Line.crx");//เพิ่ม extension 
              options.AddArguments(
"--headless=new", 
"--disable-notifications", 
"--disable-application-cache",
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

        public static void  Login_QrCode(WebDriverWait wait, System.Windows.Forms.Label label,PictureBox pic)
        {
            try
            {
                string login_qr_btn = "//*[@id='login_qr_btn']";//ปุ่มกด qr code
                string getqrcode = "//div[@class='mdCMN01Code']";

                if ( wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(login_qr_btn))).Displayed)
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
                    if (!Directory.Exists(@".\login\"))
                    {
                        Directory.CreateDirectory(@".\login\");
                    }
                    
                    if (Directory.Exists(@".\login\"))
                    {
                        bitmap.Save(@".\login\QRCode.png", System.Drawing.Imaging.ImageFormat.Png);
                        Console.WriteLine("QR Code generated successfully.");

                        pic.ImageLocation = @".\login\QRCode.png";//ดึงรูปภาพจากไฟล์
                        Console.WriteLine("กรุณาสแกน QR CODE ภายใน 2 นาที");
                    }

                }

                if (wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(getqrcode))).Displayed)
                {
                    var qrcodeTEXT = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(getqrcode))).Text;
                    ChangeUI.labelname(label, $"OTP LOGIN : {qrcodeTEXT}");
                    label.ForeColor = Color.Green;
                    Console.WriteLine(qrcodeTEXT);
                  

                }//ดึงรหัส otp 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : loginwithqrcode \t:" + ex.Message);
            }//
        }

        public static void Sendmessage_TolineNotify(string tokenlinenotify, string msg)
        {

            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}", msg);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + tokenlinenotify);

                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Console.WriteLine(responseString.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public static void ImplicitWait(ChromeDriver driver, int timeoutInSeconds)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(timeoutInSeconds);
        }

        public static void Wait_and_Click(ChromeDriver driver, By locator, int timeoutInSeconds,bool click = false,int waiting = 5)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                var checkelement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
                if (click)
                {
                    checkelement.Click();
                    Thread.Sleep(waiting * 1000);
                    return;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error WaitandClick {locator}\t\n{ex.Message}");
            }
            
        }

        public static IWebElement FindElementIfExists(IWebDriver driver, By by)
        {
            var elements = driver.FindElements(by);
            return (elements.Count >= 1) ? elements.First() : null;
        }
        public static bool IsElementPresent(IWebDriver driver, By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
