using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebCam;
using WebCam.Interfaces;

namespace WebCam
{
    public class settings2
    {
          
        public int hue_max2 { get; set; }
        public int hue_min2 { get; set; }
        public int first_number { get; set; }
        public int second_number { get; set; }

       
    }


    public class Red_Color : IRed_Color
    {
        public int red_value_R { get; set; } = 220;
        public int green_value_R { get; set; } = 30;
        public int blue_value_R { get; set; } = 30;


    }

    public class Green_Color : IGreen_Color
    {

        public int red_value_G { get; set; } = 5;
        public int green_value_G { get; set; } = 240;
        public int blue_value_G { get; set; } = 5;
    }

    public class Blue_Color : IBlue_Color
    {

        public int red_value_B { get; set; } = 30;
        public int green_value_B { get; set; } = 30;
        public int blue_value_B { get; set; } = 240;

    }



    public class settings
    {
        

        #region HSL values
        public static int hue_max2 { get; set; }
        public static int hue_min2 { get; set; }
        public static int first_number { get; set; }
        public static int second_number { get; set; }
        #endregion

        #region Red color 
        public static int red_value_R { get; set; }
        public static int green_value_R { get; set; }
        public static int blue_value_R { get; set; }
        #endregion

        #region Green color
        public static int red_value_G { get; set; }
        public static int green_value_G { get; set; }
        public static int blue_value_G { get; set; }
        #endregion

        #region Blue color
        public static int red_value_B { get; set; } 
        public static int green_value_B { get; set; } 
        public static int blue_value_B { get; set; } 
        #endregion

        #region Radius R_B
        public static int radius_R_B { get; set; }
        #endregion

        #region Radius G
        public static int radius_G { get; set; }
        #endregion

        #region Blob filter - parameters
        public static int Blob_filter_min_width { get; set; }
        public static int Blob_filter_min_height { get; set; }
        public static bool Filter_Blobs { get; set; } = true;


        //public static bool Filter_Blobs 

        //{
        //    get { return Filter_Blobs; }
        //    set { Filter_Blobs = true; }

        //}
        #endregion

        #region Pen parameters
        public static int First_pen_color { get; set; } = 252;
          public static int Second_pen_color { get; set; } = 3;
          public static int Third_pen_color { get; set; } = 26;
          public static int Pen_width { get; set; } = 2;
          public static int First_point { get; set; } = 250;
          public static int Second_point { get; set; } = 1;
          public static int Pen_font { get; set; } = 12;
          public static string Pen_font2 { get; set; } = "Arial";
          public static int Pen_second_width { get; set; } = 5;
        #endregion

      


        public settings()
        {

           
            #region HSL values
              hue_max2 = 128;
              hue_min2 = 158;
              first_number = (int)(double)0.6f;
              second_number = (int)(double)0.1f;
            #endregion

            #region Red color
              red_value_R = 220;
              green_value_R = 30;
              blue_value_R = 30;
            #endregion

            #region Green color
              red_value_G = 5;
              green_value_G = 240;
              blue_value_G = 5;
            #endregion

            #region Blue color
              red_value_B = 30;
              green_value_B = 30;
              blue_value_B = 240;
            #endregion

            #region Radius R_B
              radius_R_B = 100;
            #endregion

            #region Radius G
              radius_G = 180;
            #endregion

            #region Blob filter - parameters

            //Blob_filter_min_width = 5;   //5
            //Blob_filter_min_height = 5;   //5
            #endregion


        }
    }

    public class gray_values
    {
        public static double First_gray_color { get; set; } = 0.2125;
        public static double Second_gray_color { get; set; } = 0.7154;
        public static double Third_gray_color { get; set; } = 0.0721;


    }

    public class distance
    {
        public static int first_parameter { get; set; }
        public static int second_parameter { get; set; }
        public static int third_parameter { get; set; }
        public static int fourth_parameter { get; set; }

    }

    public class const_par
    {
        public static int number { get; set; } = 2;
    }

    public class px_value // conversion from 1px
    {
        public static double numberCM { get; set; } = 0.0264583333;   
        public static double numberMM { get; set; } = 0.2645833333;   

    }

    public class rect_par : IBlob_parameters
    {


        public int first_rectangle_X { get; set; }
        public int first_rectangle_Y { get; set; }
        public int second_rectangle_X { get; set; }
        public int second_rectangle_Y { get; set; }

        public double distanceCM { get; set; }
        public double distanceMM { get; set; }

        public string value_1 { get; set; }
        public string value_2 { get; set; }
        public string values { get; set; }

        public int operand1;
        public int operand2;
        public int subtraction_X 
        { 
            get { return operand1; }
            set { operand1 = first_rectangle_X - second_rectangle_X; }
        }
        public int subtraction_Y
        {
            get { return operand2; }
            set { operand2 = first_rectangle_Y - second_rectangle_Y; }
        }

        
    }


}
