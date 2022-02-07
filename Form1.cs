using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using WebCam;


# region libraries AForge.NET
using AForge;
using AForge.Video.DirectShow;
using AForge.Video;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;

#endregion



namespace WebCam
{
    

    public partial class Form1 : Form
    {
        
        
        


        public Form1 ()
        {
            InitializeComponent();

        }
        
        #region Global variables
        public FilterInfoCollection Web_Camera { get; set; }
        public VideoCaptureDevice Camera { get; set; }

        int hue_max, hue_min, threshold_value, Radius, red, green, blue, minimal_width, minimal_height, maximum_height, maximum_width;
        Graphics graphics;
        Rectangle objectRect;
        int objectX, objectY;
        string dist = "The distance is";


        Bitmap source_video, color_filtering_video, binary_video, grey_image_video, edge_video;

        #endregion

        #region Pen properties
           Pen Pen_color = new Pen(Color.FromArgb(settings.First_pen_color, settings.Second_pen_color, settings.Third_pen_color), settings.Pen_width);
           SolidBrush Pen_brush = new SolidBrush(Color.Red);
           Font Pen_font = new Font(settings.Pen_font2, settings.Pen_font, FontStyle.Bold);
           Pen Pen_second_color = new Pen(Color.Red, settings.Pen_second_width);
           System.Drawing.Point Pen_point = new System.Drawing.Point(settings.First_point, settings.Second_point);
        #endregion

        #region Filters for AFORGE.NET

        BlobCounter blob_filter;
        Grayscale grayscale;
        Threshold threshold;

        #region Edge detectors
        CannyEdgeDetector canny;
        SobelEdgeDetector sobel;
        HomogenityEdgeDetector homo;
        DifferenceEdgeDetector diff;
        #endregion

        #endregion

        #region another methods


        private void radio_falsed()
        {
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;
            radioButton5.Enabled = false;
            radioButton6.Enabled = false;
        }

        private void radio_enabled()
        {
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            radioButton3.Enabled = true;
            radioButton4.Enabled = true;
            radioButton5.Enabled = true;
            radioButton6.Enabled = true;
        }

        public void cam_settings()
        {
            Web_Camera = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            
            foreach (FilterInfo VideoCaptureDevice in Web_Camera)

            {
                comboBox1.Items.Add(VideoCaptureDevice.Name);
            }

            do
            {
                comboBox1.SelectedIndex = 0;
            }
            while (comboBox1.SelectedIndex > 0);

            Camera = new VideoCaptureDevice();

        }

        #endregion

        #region Combobox properties
        
       

        private void Combobox2_properties ()
        {
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(new object[]

                {
                   "0. HSL filter ",
                   "1. Grayscale filter",
                   "2. Threshold filter",
                   "3. Euclidean Color Filtering (RGB)",
                   "4. ... * ",
                   "5. ....",
                   "6. ....",
                   "7. ....",
                }
            );

        }
        #endregion

        public void Form1_Load(object sender, EventArgs e)
        {
            label17.Text = ($"Rozmery PictureBoxu sú nasledovné: X-ová os je :" + pictureBox2.Width +
                " a Y-ová os má hodnotu: " + pictureBox2.Height + ".");
            radio_falsed();
   
            
            try

            {
                 cam_settings();


            }


            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }


        public void Start (object sender, EventArgs e) 
        {
            Camera = new VideoCaptureDevice(Web_Camera[comboBox1.SelectedIndex].MonikerString);
            Camera.NewFrame += new AForge.Video.NewFrameEventHandler(get_Frame);
            Camera.Start();
            Combobox2_properties();
           
        }

       

        private void get_Frame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap source_video = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = source_video;
            pictureBox2.Image = color_filtering_video;
            pictureBox3.Image = binary_video;        
        }

        public void cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap source_video = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = source_video;
            pictureBox2.Image = color_filtering_video;
        }

        private void Reset (object sender, EventArgs e)  
        {
            Camera.Stop();
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
        }

        private void Pausa (object sender, EventArgs e)  
        {
            Camera.Stop();
        }

        private void Main_colors(object sender, EventArgs e) 
        {       

            if (comboBox2.SelectedIndex == 0)   
            {
                Camera.NewFrame += HSL_green_color;
            }

            if (comboBox2.SelectedIndex == 1) 
            {
                pictureBox2.Image = null;

                Camera.NewFrame += Grayscale;
            }

            if (comboBox2.SelectedIndex == 2)  
            {
                pictureBox2.Image = null;
                Camera.NewFrame += Threshold;
            }

            if (comboBox2.SelectedIndex == 3) 
            {
                pictureBox2.Image = null;
                radio_enabled();

                Camera.NewFrame += Euclidean_RED;
                Camera.NewFrame += Euclidean_GREEN;
                Camera.NewFrame += Euclidean_BLUE;


            }


        }


        private void Threshold (object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap video1 = (Bitmap)eventArgs.Frame.Clone();
            Bitmap video2 = (Bitmap)eventArgs.Frame.Clone();
            radio_falsed();


            //grayscale = new Grayscale(0.2125, 0.7154, 0.0721);
            grayscale = new Grayscale(gray_values.First_gray_color, gray_values.Second_gray_color, gray_values.Third_gray_color);
            video2 = grayscale.Apply(video2);

            
            threshold = new Threshold();
            threshold.ThresholdValue = threshold_value;

            video2 = threshold.Apply(video2);
            threshold.ApplyInPlace(video2);
            
            


            Bitmap a = AForge.Imaging.Image.Clone(
            new Bitmap(video2),
            PixelFormat.Format24bppRgb);
            AForge.Imaging.Image.Convert8bppTo16bpp(a);
           
            pictureBox2.Image = video2;
        }

        private void Grayscale (object sender, NewFrameEventArgs eventArgs)
        {

            Bitmap video1 = (Bitmap)eventArgs.Frame.Clone();
            Bitmap video2 = (Bitmap)eventArgs.Frame.Clone();

            radio_falsed();

            grayscale = new Grayscale(gray_values.First_gray_color, gray_values.Second_gray_color, gray_values.Third_gray_color);
            video2 = grayscale.Apply(video2);
            //pictureBox2.Image = video2;
           
            canny = new CannyEdgeDetector();
            video2 = canny.Apply(video2);
            pictureBox2.Image = video2;
        }


        public void HSL_green_color (object sender, NewFrameEventArgs eventArgs)
        {
            new settings();
            

            Bitmap color_filtering_video = (Bitmap)eventArgs.Frame.Clone();
            Bitmap color_filtering_video2 = (Bitmap)eventArgs.Frame.Clone();

            //radio_enabled(); 
            //radio_falsed();

            HSLFiltering hSLfiltering = new HSLFiltering();
            //HSLFiltering hSLfiltering = new HSLFiltering();
            /* hSLfiltering.Hue = new IntRange(hue_value, 15);
             hSLfiltering.Saturation = new Range(0.6f, 1);
             hSLfiltering.Luminance = new Range(0.1f, 1);
            */

            //HSLFiltering greenFilter = new HSLFiltering(new IntRange(hue_value, 15), new Range(0.400f, 1.0f), new Range(0.15f, 1.0f));
            //greenFilter.ApplyInPlace(video2);

            //settings2 demo = new settings2();

            //hSLfiltering.Hue = new IntRange(demo.hue_max2, demo.hue_min2);

            hSLfiltering.Hue = new IntRange(settings.hue_max2, settings.hue_min2);
            hSLfiltering.Saturation = new IntRange(settings.first_number, 1);
            hSLfiltering.Luminance = new IntRange(settings.second_number, 1);
            hSLfiltering.ApplyInPlace(color_filtering_video);
            pictureBox2.Image = color_filtering_video;

            hSLfiltering.Hue = new IntRange(settings.hue_max2, settings.hue_min2);
            hSLfiltering.Saturation = new IntRange(settings.first_number, 1);
            hSLfiltering.Luminance = new IntRange(settings.second_number, 1);
            hSLfiltering.ApplyInPlace(color_filtering_video2);

            grayscale = new Grayscale(gray_values.First_gray_color, gray_values.Second_gray_color, gray_values.Third_gray_color);
            grey_image_video = grayscale.Apply(color_filtering_video2);
            edge_detectors();


        }


        private void label14_Click(object sender, EventArgs e)
        {

        }

  

        private void button4_Click(object sender, EventArgs e)
        {
            Camera.NewFrame += Euclidean_RED;
            Camera.NewFrame += Euclidean_GREEN;
            Camera.NewFrame += Euclidean_BLUE;
        }

      

        private void Euclidean_RED (object sender, NewFrameEventArgs eventArgs)
        {
            if (radioButton1.Checked)
            {
                new settings();

                Bitmap color_filtering_video = (Bitmap)eventArgs.Frame.Clone();
                Bitmap color_filtering_video2 = (Bitmap)eventArgs.Frame.Clone();
 

                EuclideanColorFiltering filter_red = new EuclideanColorFiltering();
                rect_par rect = new rect_par();

                Red_Color red = new Red_Color();             
                filter_red.CenterColor = new RGB((byte)red.red_value_R,
                    (byte)red.green_value_R, (byte)red.blue_value_R);


                filter_red.Radius = (short)settings.radius_R_B;
                filter_red.ApplyInPlace(color_filtering_video);
                pictureBox2.Image = color_filtering_video;


                 filter_red.CenterColor = new RGB((byte)red.red_value_R,
                     (byte)red.green_value_R, (byte)red.blue_value_R);    


                filter_red.Radius = (short)settings.radius_R_B;
                filter_red.ApplyInPlace(color_filtering_video2);
                grayscale = new Grayscale(gray_values.First_gray_color, gray_values.Second_gray_color, gray_values.Third_gray_color);
                grey_image_video = grayscale.Apply(color_filtering_video2);
                edge_detectors();

            }


        }
 

        #region Edge detectors filters

        private void threshold_filter ()
        {
            threshold = new Threshold();  
            threshold.ThresholdValue = threshold_value;
            binary_video = threshold.Apply(edge_video);
        }

        private void difference_filter()
        {
            diff = new DifferenceEdgeDetector();
            edge_video = diff.Apply(grey_image_video);
            threshold_filter();
        }

        private void homogenity_filter()
        {
            homo = new HomogenityEdgeDetector();
            edge_video = homo.Apply(grey_image_video);
            threshold_filter();
        }

        private void sobel_filter()
        {
            sobel = new SobelEdgeDetector();
            edge_video = sobel.Apply(grey_image_video);
            threshold_filter();
        }

        private void canny_filter()
        {
            canny = new CannyEdgeDetector();
            edge_video = canny.Apply(grey_image_video);
            threshold_filter();
        }

        private void edge_detectors()
        {
            if (radioButton4.Checked)
            {
                sobel_filter();
            }

            if (radioButton5.Checked)
            {
                canny_filter();
            }

            if (radioButton6.Checked)
            {
                homogenity_filter();
            }

            if (radioButton11.Checked)
            {
                difference_filter();
            }

        }

        #endregion

    

        private void Euclidean_GREEN (object sender, NewFrameEventArgs eventArgs)
                  
            {
                if (radioButton2.Checked)

                {

                new settings();
                Bitmap color_filtering_video = (Bitmap)eventArgs.Frame.Clone();
                Bitmap color_filtering_video2 = (Bitmap)eventArgs.Frame.Clone();

                EuclideanColorFiltering filter_green = new EuclideanColorFiltering();

                Green_Color green = new Green_Color();
                filter_green.CenterColor = new RGB((byte)green.red_value_G,
                    (byte)green.green_value_G, (byte)green.blue_value_G);


                filter_green.Radius = (short)settings.radius_G;
                filter_green.ApplyInPlace(color_filtering_video);
                pictureBox2.Image = color_filtering_video;



                filter_green.CenterColor = new RGB((byte)green.red_value_G,
                    (byte)green.green_value_G, (byte)green.blue_value_G);

                filter_green.Radius = (short)settings.radius_G;
                filter_green.ApplyInPlace(color_filtering_video2);

                grayscale = new Grayscale(gray_values.First_gray_color, gray_values.Second_gray_color, gray_values.Third_gray_color);
                grey_image_video = grayscale.Apply(color_filtering_video2);
                edge_detectors();
                

            }
  

        }       

        public void listing_coordinates()
        {
            this.Invoke((MethodInvoker)delegate
            {
                richTextBox1.Text = objectRect.Location.ToString() + "\n" + richTextBox1.Text + "\n"; ;
            });
        }

        
      

        public void Euclidean_BLUE (object sender, NewFrameEventArgs eventArgs)
        {
            if (radioButton3.Checked)
            {

                new settings();
                Bitmap color_filtering_video = (Bitmap)eventArgs.Frame.Clone();
                Bitmap color_filtering_video2 = (Bitmap)eventArgs.Frame.Clone();

                EuclideanColorFiltering filter_blue = new EuclideanColorFiltering();

                //Blue_Color blue = new Blue_Color();
                //filter_blue.CenterColor = new RGB((byte)blue.red_value_B,
                //     (byte)blue.green_value_B, (byte)blue.blue_value_B);

                filter_blue.CenterColor = new RGB ((byte)settings.red_value_B, (byte)settings.green_value_B, (byte)settings.blue_value_B);

                filter_blue.Radius = (short)settings.radius_R_B;
                filter_blue.ApplyInPlace(color_filtering_video);
                pictureBox2.Image = color_filtering_video;

                filter_blue.CenterColor = new RGB((byte)settings.red_value_B,(byte)settings.green_value_B, (byte)settings.blue_value_B);
                //filter_blue.CenterColor = new RGB((byte)blue.red_value_B,
                //     (byte)blue.green_value_B, (byte)blue.blue_value_B);

                filter_blue.Radius = (short)settings.radius_R_B;
                filter_blue.ApplyInPlace(color_filtering_video2);

                grayscale = new Grayscale(gray_values.First_gray_color, gray_values.Second_gray_color, gray_values.Third_gray_color);
                grey_image_video = grayscale.Apply(color_filtering_video2);
                edge_detectors();



            }



            if (radioButton7.Checked) // Detecting one object
            {

                blob_filter = new BlobCounter();

                new settings();

                blob_filter.MinWidth = minimal_height;
                blob_filter.MinHeight = minimal_width;
                //blob_filter.MaxWidth = maximum_width;
                //blob_filter.MaxHeight = maximum_height;

                blob_filter.FilterBlobs = settings.Filter_Blobs;
                
                blob_filter.ProcessImage(binary_video);

                Rectangle[] rects = blob_filter.GetObjectsRectangles();
                Blob[] blobs = blob_filter.GetObjectsInformation();
                Bitmap color_filtering_video = (Bitmap)eventArgs.Frame.Clone();

                foreach (Rectangle recs in rects)
                {
                    
                    if (rects.Length > 0)
                    {
      
                        objectRect = rects[0];
             
                        graphics = pictureBox1.CreateGraphics();

                        {
                            graphics.DrawRectangle(Pen_color, objectRect);
                        }

                        objectX = objectRect.X + (objectRect.Width / const_par.number);
                        objectY = objectRect.Y + (objectRect.Height / const_par.number);

                        graphics.DrawString(objectX.ToString() + " X " + objectY.ToString(), Pen_font, Pen_brush, Pen_point);
                        graphics.Dispose();

                        if (radioButton7.Checked)
                        {
                            listing_coordinates();
                        }
                    }
                }

            }

            if (radioButton8.Checked)  // Detecting more objects
            {
                new settings();

                blob_filter = new BlobCounter();

                blob_filter.MinWidth = minimal_height;
                blob_filter.MinHeight = minimal_width;

                blob_filter.FilterBlobs = settings.Filter_Blobs;
                blob_filter.ProcessImage(binary_video);

                Rectangle[] rects = blob_filter.GetObjectsRectangles();
                Blob[] blobs = blob_filter.GetObjectsInformation();

                for (int i = 0; rects.Length > i; i++)
                {
                    objectRect = rects[i];
            
                    graphics = pictureBox1.CreateGraphics();
                    {
                        graphics.DrawRectangle(Pen_color, objectRect);
                        graphics.DrawString((i + 1).ToString(), Pen_font, Pen_brush, objectRect);
                    }
                   
                    objectX = objectRect.X + (objectRect.Width / const_par.number);
                    objectY = objectRect.Y + (objectRect.Height / const_par.number);
                    graphics.DrawString(objectX.ToString() + " X " + objectY.ToString(), Pen_font, Pen_brush, Pen_point);

                    if (radioButton8.Checked)
                    {
                        listing_coordinates();
                    }

                    if (radioButton9.Checked)  // Measure the distance
                    {
                        if (rects.Length > 1)
                        {
                            for (int j = 0; j < rects.Length - 1; j++)
                            {
                                
                                distance.first_parameter = (rects[j].Left + rects[j].Right) / const_par.number;  
                                distance.second_parameter = (rects[j].Top + rects[j].Bottom) / const_par.number;  
                                distance.third_parameter = (rects[j + 1].Right + rects[j + 1].Left) / const_par.number; 
                                distance.fourth_parameter = (rects[j + 1].Bottom + rects[j + 1].Top) / const_par.number;  

                                graphics = pictureBox1.CreateGraphics();
            
                                graphics.DrawLine(Pen_second_color, distance.first_parameter, distance.second_parameter, distance.third_parameter, distance.fourth_parameter);
                            }
                        }

                        if (rects.Length == const_par.number)
                        {
                            Bitmap color_filtering_video = (Bitmap)eventArgs.Frame.Clone();

                            rect_par rect = new rect_par();

                            Rectangle first_rect = rects[0];  
                            Rectangle second_rect = rects[1];

                            rect.first_rectangle_X = first_rect.X + (first_rect.Width / const_par.number);
                            rect.first_rectangle_Y = first_rect.Y + (first_rect.Height / const_par.number);
                            rect.second_rectangle_X = second_rect.X + (second_rect.Width / const_par.number);
                            rect.second_rectangle_Y = second_rect.Y + (second_rect.Height / const_par.number);

                            rect.subtraction_X = 0;
                            rect.subtraction_Y = 0;


                             if (radioButton13.Checked)  //cm
                             {
                                 rect.distanceCM = Math.Floor((Math.Sqrt((Math.Pow((rect.subtraction_X), const_par.number)) + Math.Pow((rect.subtraction_Y), const_par.number))) * px_value.numberCM);

                                 this.Invoke((MethodInvoker)delegate
                                 {
                                     richTextBox2.Text = dist + rect.distanceCM.ToString() + " cm\n" + richTextBox2.Text + " cm\n";

                                 });
                             }

                             if (radioButton14.Checked) //mm
                             {
                                 rect.distanceMM = Math.Floor((Math.Sqrt((Math.Pow((rect.subtraction_X), const_par.number)) + Math.Pow((rect.subtraction_Y), const_par.number))) * px_value.numberMM);

                                 this.Invoke((MethodInvoker)delegate
                                 {
                                     richTextBox2.Text = dist + rect.distanceMM.ToString() + " mm\n" + richTextBox2.Text + " mm\n";

                                 });
                             }

                            rect.value_1 = "X-" + Convert.ToString(rect.subtraction_X);
                            rect.value_2 = "Y-" + Convert.ToString(rect.subtraction_Y);


                            rect.values = rect.value_1 + " " + rect.value_2;
                            BitmapData Objects_Data = color_filtering_video.LockBits(new Rectangle(0, 0, color_filtering_video.Width, color_filtering_video.Height), ImageLockMode.ReadOnly, color_filtering_video.PixelFormat);
                            Drawing.Line(Objects_Data, new IntPoint((int)rect.first_rectangle_X, (int)rect.first_rectangle_Y), new IntPoint((int)rect.second_rectangle_X, (int)rect.second_rectangle_Y), Color.Blue);


                        }

                    }


                }


            }

    }

 
    


        private void label17_Click(object sender, EventArgs e)
        {
            label17.Text = ("Rozmery PictureBoxu sú nasledovné: X-ová os je :" + pictureBox2.Width + " a Y-ová os má hodnotu: " + pictureBox2.Height + ".");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        #region trackbars

        private void trackBar9_Scroll(object sender, EventArgs e)
        {
            label8.Text = trackBar9.Value.ToString();
            red = trackBar9.Value;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {

        }

        #region Blob parameters
        public void Maximum_height_Blob(object sender, EventArgs e)
        {
            maximum_height = (int)numericUpDown3.Value;
        }

        public void Maximum_width_Blob(object sender, EventArgs e)
        {
            maximum_width = (int)numericUpDown4.Value;
        }

        public void Minimal_height_Blob(object sender, EventArgs e)
        {
           

            minimal_height = (int)numericUpDown2.Value;
        }

        public void Minimal_Width_Blob(object sender, EventArgs e)
        {
            minimal_width = (int)numericUpDown1.Value;
           
        }
        #endregion
        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            label7.Text = trackBar8.Value.ToString();
            green = trackBar8.Value;
        }

        private void trackBar10_Scroll(object sender, EventArgs e)
        {
            label6.Text = trackBar10.Value.ToString();
            blue = trackBar10.Value;
        }


        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label3.Text = trackBar2.Value.ToString();
            hue_max = trackBar2.Value;


        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            label4.Text = trackBar3.Value.ToString();
            hue_min = trackBar3.Value;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            label5.Text = trackBar4.Value.ToString();
            threshold_value = trackBar4.Value;
        }



        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            label16.Text = trackBar5.Value.ToString();
            Radius = trackBar5.Value;

        }


        public void Second_pictureBox_Click(object sender, EventArgs e)
        {

        }



        private void label2_Click(object sender, EventArgs e)
        {

        }
        #endregion
      
    }
}

