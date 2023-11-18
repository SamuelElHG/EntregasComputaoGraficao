using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MultiPassGaussianBlur : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        //-1. Crear objeto de ejecucion de funciones de render (Command Buffer)
        //1. Tomar un pantallazo
        //  a. [X] Crear Textura para guardar pantallazo
        //      b.[X] Saber Ancho, Alto, Formato y color de la textura de la pantalla 
        //2. Aplicar cambios al pantallazo
        //  a. Cargar el shader de modificacion
        //  b. crear material a partir del shader cargado
        //  c. configurar material creado
        //3. Devolver el pantallazo a la camara
        //4. Ejecutar Command Buffer

        private RTHandle temporaryTexture; // Textura donde se guardara pantallazo
        private Material blurMaterial;

        public CustomRenderPass(Material blurMaterial)
        {
            this.blurMaterial = blurMaterial;
        }
        
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            //Datos que describen la textura de la pantalla (ej: Ancho, Alto, Formato de color, etc)
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;
            //Asigna una cantidad de memoria en GPU para la textura del pantallazo
            RenderingUtils.ReAllocateIfNeeded(ref temporaryTexture, descriptor, name:"_GaussianBlurTemporaryTexture");
        }
        
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            RTHandle cameraColorBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle;
            if (temporaryTexture.rt == null || cameraColorBuffer.rt == null) return;
            if (blurMaterial == null) return;

            //Objeto de lista de comandos para ejecutar despues
            CommandBuffer cmd = CommandBufferPool.Get("Multi Pass Gaussian Blur");
            
            //Tomar Pantallazo y guardarlo en la textura que se creo previamente (temporaryTexture)
            cmd.Blit(cameraColorBuffer, temporaryTexture);
            
            //Aplicar shader de modificacion al pantallazo
            cmd.Blit(temporaryTexture, cameraColorBuffer, blurMaterial, blurMaterial.FindPass("Universal Forward"));
            
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    [SerializeField] private float pixelOffset;
    [SerializeField][Range(0,16)] private int passCount;
    [SerializeField] private Material blurMaterial;
    
    CustomRenderPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(blurMaterial);

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


