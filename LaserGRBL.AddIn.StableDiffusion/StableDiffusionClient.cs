using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LaserGRBL.AddIn.StableDiffusion
{
    internal class StableDiffusionClient
    {
        //Steps: 10, Sampler: DPM++ 2M, Schedule type: Karras, CFG scale: 7, Seed: -1245, Size: 512x512, Model hash: 54ef3e3610, Model: meinamix_meinaV11, Lora hashes: "pensketch_without_pen_lora: 10b2db573492", Version: v1.9.3-amd-29-g0bde866e
        public class AIRequest
        {
            public string prompt { get; set; }
            /*
            public string negative_prompt { get; set; }
            public string[] styles { get; set; }
            */
            public int seed { get; set; } = -1245;
            /*
            public int subseed { get; set; }
            public int subseed_strength { get; set; }
            public int seed_resize_from_h { get; set; }
            public int seed_resize_from_w { get; set; }
            */

            public string sampler_name { get; set; } = "DPM++ 2M";
            public string scheduler { get; set; } = "Karras";
            /*
            public int batch_size { get; set; }
            public int n_iter { get; set; }
            */
            public int steps { get; set; } = 10;
            public int cfg_scale { get; set; } = 7;
            public int width { get; set; } = 512;
            public int height { get; set; } = 512;
            /*
            public bool restore_faces { get; set; }
            public bool tiling { get; set; }
            public bool do_not_save_samples { get; set; }
            public bool do_not_save_grid { get; set; }
            public int eta { get; set; }
            public int denoising_strength { get; set; }
            public int s_min_uncond { get; set; }
            public int s_churn { get; set; }
            public int s_tmax { get; set; }
            public int s_tmin { get; set; }
            public int s_noise { get; set; }
            public object override_settings { get; set; }
            public bool override_settings_restore_afterwards { get; set; }
            public string refiner_checkpoint { get; set; }
            public int refiner_switch_at { get; set; }
            public bool disable_extra_networks { get; set; }
            public string firstpass_image { get; set; }
            public object comments { get; set; }
            public bool enable_hr { get; set; }
            public int firstphase_width { get; set; }
            public int firstphase_height { get; set; }
            public int hr_scale { get; set; }
            public string hr_upscaler { get; set; }
            public int hr_second_pass_steps { get; set; }
            public int hr_resize_x { get; set; }
            public int hr_resize_y { get; set; }
            public string hr_checkpoint_name { get; set; }
            public string hr_sampler_name { get; set; }
            public string hr_scheduler { get; set; }
            public string hr_prompt { get; set; }
            public string hr_negative_prompt { get; set; }
            public string force_task_id { get; set; }
            public string sampler_index { get; set; }
            public string script_name { get; set; }
            public string[] script_args { get; set; }
            public bool send_images { get; set; }
            public bool save_images { get; set; }
            public object alwayson_scripts { get; set; }
            public string infotext { get; set; }
            */
        }

        /*
         {
  "progress": 0,
  "eta_relative": 0,
  "state": {},
  "current_image": "string",
  "textinfo": "string"
} 
        */

        public class AIProgress
        {
            public double progress { get; set; }
            public double eta_relative { get; set; }
            public object state { get; set; }
            public string current_image { get; set; }
            public string textinfo { get; set; }
        }


        public class AIResponse
        {
            public string[] images { get; set; }
            public object parameters { get; set; }
            public string info { get; set; }
        }

        public static void GenerateImage(string text, Action<Bitmap> completedCallback, Action<AIProgress> progressCallback)
        {
            string URI = "http://127.0.0.1:7860/sdapi/v1";
            bool inProgress = true;
            Task.Factory.StartNew(() =>
            {
                inProgress = true;
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        string parameters = JsonConvert.SerializeObject(new AIRequest
                        {
                            prompt = text
                        });
                        byte[] HtmlResult = wc.UploadData(URI + "/txt2img", "POST", Encoding.UTF8.GetBytes(parameters));
                        AIResponse result = JsonConvert.DeserializeObject<AIResponse>(Encoding.UTF8.GetString(HtmlResult));
                        if (result?.images?.Length > 0)
                        {
                            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(result.images[0])))
                            {
                                Bitmap bmp = new Bitmap(ms);
                                completedCallback.Invoke(bmp);
                            }
                        }
                    }
                }
                catch
                {
                }
                inProgress = false;
            });

            Task.Factory.StartNew(() =>
            {
                while (inProgress)
                {
                    using (WebClient wc = new WebClient())
                    {
                        byte[] HtmlResult = wc.DownloadData(URI + "/progress");
                        AIProgress result = JsonConvert.DeserializeObject<AIProgress>(Encoding.UTF8.GetString(HtmlResult));
                        progressCallback.Invoke(result);
                    }
                    Thread.Sleep(1000);
                }
                progressCallback.Invoke(new AIProgress
                {
                    progress = 0,
                    eta_relative = 0
                });
            });
        }

    }
}
