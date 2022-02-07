using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebCam.Interfaces
{
     interface IRed_Color
    {
        int red_value_R { get; set ; }
        int green_value_R { get; set; }
        int blue_value_R { get; set; }

    }

   

    interface IGreen_Color
    {
        int red_value_G { get; set; }
        int green_value_G { get; set; }
        int blue_value_G { get; set; }

    }

    interface IBlue_Color
    {
        int red_value_B { get; set; }
        int green_value_B { get; set; }
        int blue_value_B { get; set; }

    }

    interface IBlob_parameters
    {
        int first_rectangle_X { get; set; }
        int first_rectangle_Y { get; set; }
        int second_rectangle_X { get; set; }
        int second_rectangle_Y { get; set; }

        double distanceCM { get; set; }
        double distanceMM { get; set; } 

        int subtraction_X { get; set; }
        int subtraction_Y { get; set; }
        string value_1 { get; set; }
        string value_2 { get; set; }
        string values { get; set; }


    }


}
