using System.Reflection;
using DotNetWebScrapperEmpregos.models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

class Program
{
    static void Main()
    {
        IWebDriver driver = new ChromeDriver();
        Console.Clear();
        driver.Navigate().GoToUrl("https://www.empregos.com.br/vagas/desenvolvedor-net/");
        
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
                    Console.WriteLine($"- Title: {vaga.Title}");
                    Console.WriteLine($"- Description: {vaga.Description}");
                    Console.WriteLine($"- Link: {vaga.Link}");
                    Console.WriteLine(new string('-', 30));
                }
            }
            
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
