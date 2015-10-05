using ManagedCuda;
using ManagedCuda.BasicTypes;
using ManagedCuda.VectorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp1.Result;

namespace TestApp1.Algorithm
{
    class CudaSearch : CompareAlgorithm
    {
        static CudaKernel findWithCuda;
        static CudaContext cntxt;

        static void InitKernels()
        {
            if (cntxt == null || findWithCuda == null)
            {
                cntxt = new CudaContext();
                CUmodule cumodule = cntxt.LoadModule(@"C:\cudakernels\kernel.ptx");
                findWithCuda = new CudaKernel("_Z6kernelPcS_S_i", cumodule, cntxt);
            }
            
        }

        public override ResultInterpreterOpt check(ResultInterpreterOpt interpreter, String dbText, String userText)
        {
            InitKernels();
            
            int n = 10;
            int sliceIndex = 0;
            int resultIndexId = 0;
            int dbTextLength = dbText.Length;
            byte[] dbTextByte = Encoding.UTF8.GetBytes(dbText);

            CudaDeviceVariable<byte> dev_result = new CudaDeviceVariable<byte>(dbTextLength * sizeof(byte));
            CudaDeviceVariable<byte> dev_dbText = new CudaDeviceVariable<byte>(dbTextLength * sizeof(byte));
            dev_dbText.CopyToDevice(dbTextByte, 0, 0, dbTextLength * sizeof(byte));
            int maxThreads = Math.Min(cntxt.GetDeviceInfo().MaxThreadsPerBlock, dbTextLength);
            Console.Out.WriteLine(maxThreads);
            dim3 threads = new dim3(maxThreads, 1);
            dim3 blocks = new dim3((dbTextLength + maxThreads - 1) / maxThreads, 1);
            findWithCuda.GridDimensions = blocks;
            findWithCuda.BlockDimensions = threads;
            Console.Out.WriteLine("before search");
            foreach (String slice in Splitter.SplitByWindow(userText, n))
            {
                if (slice == "")
                {
                    sliceIndex++;
                    continue;
                }

                int sliceLength = slice.Length;
                byte[] sliceByte = Encoding.UTF8.GetBytes(slice);
                CudaDeviceVariable<byte> dev_slice = new CudaDeviceVariable<byte>(sliceLength * sizeof(byte));
                
                dev_slice.CopyToDevice(sliceByte, 0, 0, sliceLength * sizeof(byte));
                //Console.Out.WriteLine("before kernel");
                findWithCuda.Run(dev_dbText.DevicePointer, dev_slice.DevicePointer, dev_result.DevicePointer, sliceLength);
                //Console.Out.WriteLine("after kernel");
                byte[] result_host = new byte[dbTextLength];
                dev_result.CopyToHost(result_host);

                var indexes = new List<int>();
                for (int i = 0; i < dbTextLength; i++)
                {
                    if (result_host[i] == 1)
                    {
                        //Console.Out.Write(i);
                        indexes.Add(i);
                    }
                }
                int skipValue = slice.Split(' ')[0].Length;
                foreach (int i in indexes)
                {
                   // Console.Out.WriteLine(slice);
                    interpreter.addIndex(resultIndexId, sliceIndex + 1, sliceIndex + slice.Length + 1, i, i + slice.Length);
                    resultIndexId++;
                    // Console.Out.WriteLine();
                    //Console.Out.WriteLine(slice);
                    
                }
                sliceIndex = sliceIndex + skipValue + 1;
            }
            Console.Out.WriteLine("found");
            interpreter.getNormalizedIndexes();
            Console.Out.WriteLine("normalized");
            //Console.ReadKey();
            return interpreter;
        }


        byte[] cudaFind(String dbText, String pattern)
        {
            byte[] dbTextByte = Encoding.UTF8.GetBytes(dbText);
            byte[] patternByte = Encoding.UTF8.GetBytes(pattern);
            int dbTextLength = dbText.Length;
            int patternLength = pattern.Length;
            CudaDeviceVariable<byte> dev_dbText = new CudaDeviceVariable<byte>(dbTextLength * sizeof(byte));            
            CudaDeviceVariable<byte> dev_pattern = new CudaDeviceVariable<byte>(patternLength*sizeof(byte));
            CudaDeviceVariable<byte> dev_result = new CudaDeviceVariable<byte>(dbTextLength*sizeof(byte));

            dev_pattern.CopyToDevice(patternByte, 0, 0, patternLength*sizeof(byte));
            dev_dbText.CopyToDevice(dbTextByte, 0, 0, dbTextLength*sizeof(byte));

            int maxThreads = Math.Min(cntxt.GetDeviceInfo().MaxThreadsPerBlock, dbTextLength);
            dim3 threads = new dim3(maxThreads, 1);
            dim3 blocks = new dim3((dbTextLength + maxThreads - 1)  / maxThreads, 1);
            findWithCuda.GridDimensions = blocks;
            findWithCuda.BlockDimensions = threads;
            findWithCuda.Run(dev_dbText.DevicePointer, dev_pattern.DevicePointer, dev_result.DevicePointer, patternLength);

            byte[] result_host = new byte[dbTextLength];
            dev_result.CopyToHost(result_host);

            for (int i = 0; i < dbTextLength; i++)
            {
                Console.Out.Write(result_host[i]);
            }

            dev_dbText.Dispose();
            dev_pattern.Dispose();
            dev_result.Dispose();
            //cntxt.ClearMemory(dev_result.DevicePointer, 0, dbTextLength * sizeof(byte));
            return result_host;
        }
    }
}
