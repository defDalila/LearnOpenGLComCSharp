using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TutorialTriangulo;
public class Engine : GameWindow
{

    float[] vertices =
    {
        -0.5f, -0.5f, 0.0f, // vertice inferior esquerdo
        0.5f, -0.5f, 0.0f, // vertice inferior direito
        0.0f, 0.5f, 0.0f // vertice superior
    };

    int VertexBufferObject;
    int VertexArrayObject;

    Shader shader;


    public Engine(int width, int height, string title) : base (GameWindowSettings.Default, 
        new NativeWindowSettings()
        {
            ClientSize = (width, height),
            Title = title
        })
    {
        shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(Color4.DarkTurquoise);

        VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        shader = new Shader("Shaders/shader.vert",  "Shaders/shader.frag");

        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        shader.Use();

    }

    protected override void OnUnload()
    {
        base.OnUnload();

        shader.Dispose();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();
        GL.BindVertexArray(VertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }
}
