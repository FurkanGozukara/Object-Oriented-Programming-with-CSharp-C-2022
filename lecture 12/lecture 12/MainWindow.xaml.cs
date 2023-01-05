using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static lecture_12.customEncryption;

namespace lecture_12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            swWrite.AutoFlush = true;
            swWrite2.AutoFlush = true;
            swWrite3.AutoFlush = true;
            swWrite4.AutoFlush = true;

        }

        private void btnDataRacing_Click(object sender, RoutedEventArgs e)
        {
            dataRaceEx1();
            dataRaceEx2();
            dataRaceEx3();
        }

        StreamWriter swWrite = new StreamWriter("test.txt");
        private async Task printToFile(int _param)
        {
            Debug.WriteLine(_param.ToString());

            await swWrite.WriteLineAsync(_param.ToString());


        }

        void dataRaceEx1()
        {
            List<int> lstNumbers = Enumerable.Range(1, 100).ToList();
            foreach (var vrItemm in lstNumbers)
            {
                var _local = vrItemm;
                Task.Factory.StartNew(async () =>
                {
                    await printToFile(_local);
                });
            }
        }

        void dataRaceEx2()
        {
            List<int> lstNumbers = Enumerable.Range(1, 100).ToList();
            int irPassedData = 0;
            foreach (var vrItemm in lstNumbers)
            {
                var _local = vrItemm;
                irPassedData = vrItemm;
                Task.Factory.StartNew(() =>
                {
                    printToFile2(_local);
                    printToFile4(irPassedData);
                });
            }
        }

        void dataRaceEx3()
        {
            List<int> lstNumbers = Enumerable.Range(1, 100).ToList();
            List<object> lstNumbers2 = new List<object>();

            foreach (var item in lstNumbers)
            {
                lstNumbers2.Add(item);
            }

            List<Task> listTasks = new List<Task>();

            int _irOrder = 0;
            foreach (var vrItemm in lstNumbers2)
            {
                var _localOrder = _irOrder;
                var vrTask = Task.Factory.StartNew(() =>
                 {
                     printToFile3(vrItemm, _localOrder);
                 });
                listTasks.Add(vrTask);
                _irOrder++;
            }

            Task.WaitAll(listTasks.ToArray());//if i dont wait tasks to be finished this list will be empty by the time this line of code is executed since task were not even started at that point
            File.WriteAllLines("test5.txt", listNumbersArrived.Select(pr => pr.ToString()).ToList());
            File.WriteAllLines("test6.txt", threadSafeList.OrderBy(pr => pr.irOrder).Select(pr => pr.srVal + "\t" + pr.irOrder.ToString()).ToList());
        }



        StreamWriter swWrite2 = new StreamWriter("test2.txt");
        private void printToFile2(int _param)
        {
            lock (swWrite2)
            {
                swWrite2.WriteLine(_param.ToString());
            }
        }

        StreamWriter swWrite4 = new StreamWriter("test4.txt");
        private void printToFile4(int _param)
        {
            lock (swWrite4)
            {
                swWrite4.WriteLine(_param.ToString());
            }
        }

        struct ourSpecialParams
        {
            public string srVal;
            public int irOrder;
        }

        StreamWriter swWrite3 = new StreamWriter("test3.txt");
        private static List<int> listNumbersArrived = new List<int>();
        ConcurrentBag<ourSpecialParams> threadSafeList = new ConcurrentBag<ourSpecialParams>();
        private void printToFile3(object _param, int _irOrder = 0)
        {
            listNumbersArrived.Add(Convert.ToInt32(_param));
            threadSafeList.Add(new ourSpecialParams { srVal = "complex item " + _irOrder + " " + DateTime.Now.ToString("mm-fffff"), irOrder = _irOrder });
            //lock (swWrite3)
            //{
            //    swWrite3.WriteLine(_param.ToString());
            //}
        }

        private readonly static string myCustomKey1 = "test key example";

        private void btnHash_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.Text = txtInput.Text.sha256();

            SymmetricAlgorithm aes = new AesManaged();

            aes.Key = myCustomKey1.toByteArray().to32Bytes();

            string message = txtInput.Text;

            // Call the encryptText method to encrypt the a string and save the result to a file   
            EncryptText(aes, message, "encryptedData.txt");

            symetricencryptout.Text = File.ReadAllText("encryptedData.txt");

        }

        private void btnDecryptAes_Click(object sender, RoutedEventArgs e)
        {
            SymmetricAlgorithm aes = new AesManaged();
            aes.Key = myCustomKey1.toByteArray().to32Bytes();
            SymmetricAlgorithm aes2 = new AesManaged();
            var vrKey = "some other key".toByteArray().to32Bytes();
            aes2.Key = vrKey;

            //var vrDecryptedData = DecryptData(aes2, "encryptedData.txt");//this causes error

            var vrDecryptedData = DecryptData(aes, "encryptedData.txt");
            decyrptAes.Text = vrDecryptedData;
        }

        //asymetric encryption - maximum size is limited
        private void btnRSA1_Click(object sender, RoutedEventArgs e)
        {

            //Encrypt and export public and private keys
            var rsa1 = new RSACryptoServiceProvider();
            rsa1.KeySize = 16384;
            string privateKey = rsa1.ToXmlString(true);   // server side export
            string publicKey = rsa1.ToXmlString(false);   // server side export
            byte[] toEncryptData = Encoding.ASCII.GetBytes(txtInput.Text);
            byte[] encryptedRSA = rsa1.Encrypt(toEncryptData, false);
            string EncryptedResult = Encoding.Default.GetString(encryptedRSA);

            //Decrypt using exported keys
            var rsa2 = new RSACryptoServiceProvider();
            rsa2.FromXmlString(privateKey);
            byte[] decryptedRSA = rsa2.Decrypt(encryptedRSA, false);
            string originalResult = Encoding.Default.GetString(decryptedRSA);
        }


        //symetric encryption - big size is possible
        private void btnex2_Click(object sender, RoutedEventArgs e)
        {
            var vrMasterKey = "masterkey.txt";
            if (File.Exists(vrMasterKey) == false)
            {
                using (SymmetricAlgorithm symmetricAlgorithm = Aes.Create())
                {
                    symmetricAlgorithm.GenerateKey();
                    byte[] key = symmetricAlgorithm.Key;
                    File.WriteAllBytes(vrMasterKey, key);
                }
            }


            using (SymmetricAlgorithm symmetricAlgorithm = Aes.Create())
            {

                byte[] key = File.ReadAllBytes(vrMasterKey);


                // Encrypt some data
                string plainText = txtInput.Text;
                byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                byte[] cipherTextBytes;
                using (ICryptoTransform encryptor = symmetricAlgorithm.CreateEncryptor(key, symmetricAlgorithm.IV))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    }
                    cipherTextBytes = memoryStream.ToArray();
                }
                File.WriteAllBytes("cihperedText.txt", cipherTextBytes);
                // Decrypt the data
                string decryptedText;
                using (ICryptoTransform decryptor = symmetricAlgorithm.CreateDecryptor(key, symmetricAlgorithm.IV))
                using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            decryptedText = streamReader.ReadToEnd();
                        }
                    }
                }
                decyrptAes.Text= decryptedText; 
            }
        }
    }
}
