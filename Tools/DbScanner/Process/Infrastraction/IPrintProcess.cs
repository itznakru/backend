/*======================================================================================================================================================= 
Class:PrintProcess
Purpose: Monitor progress. The process print to console state of IProcessInfrastraction entity.         
  
======================================================================================================================================================= */
namespace DbScanner.Process.Infrastruction
{
    interface IPrintProcess
    {
        public void Run(CancellationToken ct);
    }
}