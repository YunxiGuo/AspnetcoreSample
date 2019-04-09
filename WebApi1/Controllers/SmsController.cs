using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi1.Dtos;

namespace WebApi1.Controllers
{
    [Route("api1/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "this is webapi1" };
        }

        // GET api/values/5
        /// <summary>
        /// 根据id获取对象
        /// </summary>
        /// <param name="id">唯一索引</param>
        /// <returns>对象</returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Route("Send_LX")]
        public async Task<IActionResult> Send_LX([FromBody]Send_LX input)
        {
            var phone = input.PhoneNum;
            var msg = input.Msg;
            return Ok($"电话号码:{phone},信息:{msg}");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
