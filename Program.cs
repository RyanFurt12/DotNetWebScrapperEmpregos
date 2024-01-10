using System.IO;
using System.Reflection;
using DotNetWebScrapperEmpregos.models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

class Program
{
    static void Main()
    {

        Console.Write("\nEmpregos.com\nO que esta procurando? ");
        string searchInput = Console.ReadLine() ?? "";
        searchInput = searchInput.Replace(" ", "-").Replace(".", "-").Replace("#", "").Replace("+", "");
        searchInput = searchInput.ToLower();

        string txtPath = @$"data/{searchInput}.txt";
        if (File.Exists(txtPath)) File.Delete(txtPath);
        StreamWriter sw = new StreamWriter(txtPath);

        IWebDriver driver = new ChromeDriver();
        Console.Clear();
        driver.Navigate().GoToUrl($"https://www.empregos.com.br/vagas/{searchInput}/");
        
        string numberOfPagesText = driver.FindElement(By.XPath("//*[@id='ctl00_ContentBody_pPaiResultadoTopo']/strong[2]")).Text;
        int numberOfPages = int.Parse(numberOfPagesText.Split(" ")[2]);
        int activePage = 1;

        do{
            try
            {
                IWebElement listElement = driver.FindElement(By.ClassName("list"));
                var itemElements = listElement.FindElements(By.ClassName("item"));

                foreach (var item in itemElements)
                {
                    Vaga vaga = new Vaga();
                    try{
                        // IWebElement titleArea = ;
                        vaga.Title = item.FindElement(By.TagName("h2")).FindElement(By.TagName("a")).Text;
                        vaga.Link = item.FindElement(By.TagName("h2")).FindElement(By.TagName("a")).GetAttribute("href");
                        vaga.Description = item.FindElement(By.ClassName("resumo-vaga")).Text;
                    }
                    catch (NoSuchElementException)
                    {
                        continue;
                    }
                    catch (StaleElementReferenceException)
                    {
                        continue;
                    }

                    // Print the extracted information
                    sw.WriteLine($"- Title: {vaga.Title}");
                    sw.WriteLine($"- Description: {vaga.Description}");
                    sw.WriteLine($"- Link: {vaga.Link}");
                    sw.WriteLine(new string('-', 30));
                }
            }
            catch (NoSuchElementException){}
            catch (StaleElementReferenceException){}
            
            finally
            {
                if(activePage < numberOfPages){
                    try{
                        driver.FindElement(By.XPath("//*[@id='ctl00_ContentBody_lkbPaginacaoTopProximo']")).Click();
                    }
                    catch(ElementClickInterceptedException){
                        driver.FindElement(By.XPath("//*[@id='ctl00_ContentBody_aFecharModalLead']")).Click();
                        driver.FindElement(By.XPath("//*[@id='ctl00_ContentBody_lkbPaginacaoTopProximo']")).Click();
                    }
                    finally{
                        Console.WriteLine("MUDANDO DE PAGINA");
                        Console.WriteLine(new string('-', 30));
                    }
                }

                activePage++;
            }
        }while(activePage <= numberOfPages);

        driver.Close();
    }
}
