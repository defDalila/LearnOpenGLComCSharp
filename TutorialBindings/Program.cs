using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common.Input;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Threading.Channels;

string imagesPath = "../../../Images/";


GameWindowSettings gws = GameWindowSettings.Default;
gws.UpdateFrequency = 60.0;


NativeWindowSettings nws = NativeWindowSettings.Default;
nws.Icon = LoadIcon(imagesPath, "icon.png", 225, 225);
nws.AutoLoadBindings = true;
nws.StartFocused = true;
nws.ClientSize = new Vector2i(512, 512);
nws.WindowBorder = WindowBorder.Resizable;
nws.Title = "OpenTK Lecture!";


GameWindow game = new GameWindow(gws, nws);
game.Load += delegate
{
    Console.WriteLine("Loaded!!");

    int program = CreateProgram();
    GL.UseProgram(program);

};

game.Unload += delegate
{
};

game.RenderFrame += delegate (FrameEventArgs eventArgs)
{
    GL.Clear(ClearBufferMask.ColorBufferBit);

    int VertexArrayObject = CreateBuffers(out int indexBuffer);

    GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
    GL.BindVertexArray(VertexArrayObject);
    GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, 0);

    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    GL.BindVertexArray(0);
    GL.DeleteBuffer(indexBuffer);
    GL.DeleteVertexArray(VertexArrayObject);

    game.SwapBuffers();
};

game.UpdateFrame += delegate (FrameEventArgs eventArgs)
{
   if(game.KeyboardState.IsKeyDown(Keys.Escape))
        game.Close();
};

game.Run();

static int CreateProgram()
{
    var path = "../../../Shaders/";

    int vertShaderId = GL.CreateShader(ShaderType.VertexShader);
    GL.ShaderSource(vertShaderId, File.ReadAllText(path + "vert.glsl"));
    GL.CompileShader(vertShaderId);
    Console.WriteLine(GL.GetShaderInfoLog(vertShaderId));

    int fragShaderId = GL.CreateShader(ShaderType.FragmentShader);
    GL.ShaderSource(fragShaderId, File.ReadAllText(path + "frag.glsl"));
    GL.CompileShader(fragShaderId);
    Console.WriteLine(GL.GetShaderInfoLog(fragShaderId));

    int programId = GL.CreateProgram();
    GL.AttachShader(programId, vertShaderId);
    GL.AttachShader(programId, fragShaderId);
    GL.LinkProgram(programId);
    Console.WriteLine(GL.GetProgramInfoLog(programId));

    GL.DetachShader(programId, vertShaderId);
    GL.DetachShader(programId, fragShaderId);
    GL.DeleteShader(vertShaderId);
    GL.DeleteShader(fragShaderId);

    return programId;
}

static int CreateBuffers(out int indexBuffer)
{
    float[] vertices =
    {
        -0.5f, -0.5f, 0.0f,
        0.0f, 0.5f, 0.0f,
        0.5f, -0.5f, 0.0f
    };

    float[] colors =
    {
        0.9f, 0.0f, 0.5f,
        0.0f, 0.9f, 0.5f,
        0.0f, 0.5f, 0.9f
    };

    uint[] indices =
    {
        0, 1, 2
    };

    indexBuffer = GL.GenBuffer();
    int vertexBuffer = GL.GenBuffer();
    int colorBuffer = GL.GenBuffer();
    int vertexArrayObject = GL.GenVertexArray();

    GL.BindVertexArray(vertexArrayObject);

    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
    GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),
                  vertices, BufferUsageHint.StreamCopy);
    GL.EnableVertexAttribArray(0);
    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

    GL.BindBuffer(BufferTarget.ArrayBuffer, colorBuffer);
    GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * sizeof(float),
                  colors, BufferUsageHint.StreamCopy);
    GL.EnableVertexAttribArray(1);
    GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    

    GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
    GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), 
                  indices, BufferUsageHint.StreamCopy);

      
    return vertexArrayObject;
}


static WindowIcon LoadIcon(string path, string file, int width, int height)
{
    using var image = Image.Load<Rgba32>(path + file);

    if (image.Width != width || image.Height != height)
    {
        image.Mutate(x => x.Resize(width, height));
    }

    var pixels = new byte[width * height * 4];
    image.CopyPixelDataTo(pixels);

    var iconImage = new OpenTK.Windowing.Common.Input.Image(width, height, pixels);
    return new WindowIcon(iconImage);
}

