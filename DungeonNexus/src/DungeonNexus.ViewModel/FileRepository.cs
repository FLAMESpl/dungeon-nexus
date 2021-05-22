using DungeonNexus.Infrastructure.DependencyContainer;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonNexus.ViewModel
{
    [Scoped]
    public class FileRepository
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public FileRepository(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task Upload(Stream stream, string location)
        {
            var combinedLocation = Path.Combine(hostingEnvironment.WebRootPath, location);
            var directoryLocation = Path.GetDirectoryName(combinedLocation)!;
            Directory.CreateDirectory(directoryLocation);
            await using var fileStream = new FileStream(combinedLocation, FileMode.Create);
            await stream.CopyToAsync(fileStream);
        }
    }
}
