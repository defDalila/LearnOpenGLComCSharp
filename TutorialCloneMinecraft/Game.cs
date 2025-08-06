using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TutorialCloneMinecraft;
internal class Game : GameWindow
{
    // Constantes
    private static int _width;
    private static int _height;

    // Vertices de um triângulo
    float[] vertices =
    {
        0.0f, 0.5f, 0.0f, // vertice superior
        -0.5f, -0.5f, 0.0f, // vertice infefior esq
        0.5f, -0.5f, 0.0f // vertice inferior direito
    };

    // Vars Render Pipeline
    int VertexArrayObject;
    int VertexBufferObject;
    Shader ShaderProgram;


    public Game(int width, int height) : base(GameWindowSettings.Default,
        new NativeWindowSettings() { ClientSize = (width, height)})
    {
        // Centraliza a Janela no monitor
        this.CenterWindow(new Vector2i(width, height));
        _width = width;
        _height = height;
        Title = "Clone Minecraft com OpenTK/OpenGL";

        ShaderProgram = new Shader("Default.vert", "Default.frag");
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);

        _width = e.Width;
        _height = e.Height;
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        VertexArrayObject = GL.GenVertexArray();
        VertexBufferObject = GL.GenBuffer();

        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices,
            BufferUsageHint.StaticDraw);

        // Bind com VertexArrayObject
        GL.BindVertexArray(VertexArrayObject);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexArrayAttrib(VertexArrayObject, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0); // unbinding VertexBufferObject
        GL.BindVertexArray(0); // unbind VertexArrayObject


    }

    protected override void OnUnload()
    {
        base.OnUnload();

        GL.DeleteBuffer(VertexBufferObject);
        GL.DeleteVertexArray(VertexArrayObject);
        ShaderProgram.Dispose();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.ClearColor(Color4.LightBlue);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Desenhar o Triangulo
        ShaderProgram.Use();
        GL.BindVertexArray(VertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        SwapBuffers();

    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
            Close();
    }



}
