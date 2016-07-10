using FileBrowserWeb.Managers;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FileBrowserWeb.Controllers
{
    public class FileSystemController : ApiController
    {        
        // GET: /File/path
        public HttpResponseMessage Get([FromUri] string path )
        {
            FileSystemManager fileSystemManager;

            fileSystemManager = new FileSystemManager();
                        
            if ( path == null )
            {
                return Request.CreateResponse( HttpStatusCode.OK , fileSystemManager.GetFileSystem() );
            }
            else
            {
                return Request.CreateResponse( HttpStatusCode.OK , fileSystemManager.GetFileSystem( path ) );
            }
        }       
    }
}
