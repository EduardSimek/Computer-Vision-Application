Detecting objects - Computer Vision Application

The main goal of this project is implementation algoritm, which converts a recorded image from camera to RGB filter. In this application I used algoritm Euclidean color filtering, which is part of external C# library AForge.NET. After conversion I implement blob filter, which allows rendering X,Y coordinates into component RichTextBox. Via the second RichTextBox algoritm render and writes the distance between two detected objects in units of length.

The application consists of the next parcial steps:
1. Importing frameworks AForge.NET and Accord.NET into programming language C#
2. Defining individual data types, which was used in the project.
3. Implementation of HSL colorful model, which enables to filter out all colors with the aim of obtain the final green color. 
4. Application image processing algoritms - grayscale, thresholding and implementation of Sobel edge detector. Blog filter was implemented in the input RGB image, which is recorded by digital camera Niceboy VEGA X Lite .

Flow chart of the application morphological transformation
![image](https://github.com/EduardSimek/Detecting-objects-Computer-vision-project-/assets/89217170/af7b92c9-227a-4776-a749-bc6bb52ef527)

The captured images before and after implementation of morphological transformation (opening) 
![image](https://github.com/EduardSimek/Detecting-objects-Computer-vision-project-/assets/89217170/1a8b52e3-8c45-414a-ae9e-1873ff6ab776)

Final image of detected plant by blob filter 
![image](https://github.com/EduardSimek/Detecting-objects-Computer-vision-project-/assets/89217170/a5747e99-4113-4cab-89d0-43451b2ad2bb)


