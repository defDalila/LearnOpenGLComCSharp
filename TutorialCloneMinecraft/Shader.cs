using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TutorialCloneMinecraft;
internal class Shader
{
    int ProgramID;
    private bool _disposed = false;

    public Shader(string vertPath, string fragPath)
    {

        // Cria, Vincula à fonte e Complila a VertShader
        int VertexShaderID = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShaderID, LoadShaderSource(vertPath));        
        CompileShader(VertexShaderID);

        // Cria, Vincula à fonte e Complila a FragmentShader
        int FragmentShaderID = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShaderID, LoadShaderSource(fragPath));
        CompileShader(FragmentShaderID);

        ProgramID = BuildProgram(VertexShaderID, FragmentShaderID);

    }

    public void Use()
    {
        GL.UseProgram(ProgramID);
    }

    private static void CompileShader(int shader)
    {
        GL.CompileShader(shader);

        GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
        if (code != (int)All.True)
        {
            var infoLog = GL.GetShaderInfoLog(shader);

            throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
        }
    }

    private static int BuildProgram(int vertShaderId,  int fragShaderId)
    {
        var programId = GL.CreateProgram();

        GL.AttachShader(programId, vertShaderId);
        GL.AttachShader(programId, fragShaderId);
        GL.BindFragDataLocation(programId, 0, "color");

        GL.LinkProgram(programId);


        GL.GetProgram(programId, GetProgramParameterName.LinkStatus, out var code);
        if (code != (int)All.True)
        {
            var infoLog = GL.GetProgramInfoLog(programId);
            throw new Exception($"Error occurred whilst Linking the Program({programId}).\n\n{infoLog}");
        }

        //GL.DetachShader(programId, vertShaderId);
        //GL.DetachShader(programId, fragShaderId);
        GL.DeleteShader(vertShaderId);
        GL.DeleteShader(fragShaderId);

        return programId;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            GL.DeleteProgram(ProgramID);
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

    private static string LoadShaderSource(string filePath)
    {
        string shaderSource = "";

        try
        {
            using (StreamReader reader = new StreamReader("Shaders/" + filePath))
            {
                shaderSource = reader.ReadToEnd();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load shader source file: {ex.Message}");
        }

        return shaderSource;
    }
}
