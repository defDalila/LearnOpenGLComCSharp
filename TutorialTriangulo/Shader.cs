using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TutorialTriangulo;
public class Shader
{
    int Handle;
    private bool _disposed = false;

    public Shader(string vertexPath, string fragmentPath)
    {
        int VertexShader;
        int FragmentShader;

        string VertexShaderSource = File.ReadAllText(vertexPath);
        string FragmentShaderSource = File.ReadAllText(fragmentPath);

        VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, VertexShaderSource);

        FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader, FragmentShaderSource);

        GL.CompileShader(VertexShader);
        GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int statusVertex);
        if (statusVertex == 0)
        {
            string info = GL.GetShaderInfoLog(VertexShader);
            Console.WriteLine(info);
        }

        GL.CompileShader(FragmentShader);
        GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int statusFragment);
        if (statusFragment == 0)
        {
            string info = GL.GetShaderInfoLog(FragmentShader);
        }

        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, VertexShader);
        GL.AttachShader(Handle, FragmentShader);

        GL.LinkProgram(Handle);

        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
        {
            string info = GL.GetProgramInfoLog(Handle);
            Console.WriteLine(info);
        }

        GL.DetachShader(Handle, VertexShader);
        GL.DetachShader(Handle, FragmentShader);
        GL.DeleteShader(FragmentShader);
        GL.DeleteShader(VertexShader);
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            GL.DeleteProgram(Handle);
            _disposed = true;
        }
    }

    ~Shader()
    {
        if (_disposed == false)
            Console.WriteLine("GPU Resource Leak! Didn't call Dispose()?");
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
