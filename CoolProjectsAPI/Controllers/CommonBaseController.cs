using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using ClassLibrary;
using MusicClasses;

namespace CoolProjectsAPI.Controllers
{
    public abstract class CommonBaseController : ControllerBase
    {
        protected readonly ILogger<CommonBaseController> _logger;

        public CommonBaseController(ILogger<CommonBaseController> logger)
        {
            _logger = logger;
        }
    }
}
