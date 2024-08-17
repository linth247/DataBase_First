using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;
using WebAPI.Models;
using WebAPI.Parameters;

namespace WebAPI.Services
{
    public class AsyncService
    {
        public AsyncService()
        {
            
        }

        public async Task<int> 主作業()
        {
            //int 作業1結果 = 作業1();
            //int 作業2結果 = 作業2();
            //int 作業3結果 = 作業3();            
            
            // 同時執行，非同步作業
            var 作業1結果 = 作業1Async();
            var 作業2結果 = 作業2Async();

            var onetwo = await 作業1結果 + await 作業2結果;

            // 如果必須等待1，2的結果，才繼續執行作業3
            var 作業3結果 = 作業3Async(onetwo);

            // 這邊最大的是3秒
            //int result = await 作業1結果 + await 作業2結果 + await 作業3結果;
            int result = await 作業3結果;

            return result;
        }

        private async Task<int> 作業1Async()
        {
            await Task.Delay(1000);
            return 1;
        }        
        
        private async Task<int> 作業2Async()
        {
            await Task.Delay(2000);
            return 2;
        }
        private async Task<int> 作業3Async(int i)
        {
            await Task.Delay(3000);
            return 3 * i;
        }        
        private int 作業1()
        {
            Task.Delay(1000).Wait();
            return 1;
        }        
        
        private int 作業2()
        {
            Task.Delay(2000).Wait();
            return 2;
        }
        private int 作業3()
        {
            Task.Delay(3000).Wait();
            return 3;
        }
    }
}
