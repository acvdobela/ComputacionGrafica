using System;
using Session1;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MaterialInyectorFeature1 : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        private string profilerTag;
        private RenderTargetHandle temporaryTextureHandle;
        private RenderTargetHandle temporaryTextureHandle2;
        private bool executeSucces;

        public CustomRenderPass(string profilerTag)
        {
            this.profilerTag = profilerTag;
            temporaryTextureHandle.Init("_TEMP_MATERIALINJECTOR_TARGET");
            temporaryTextureHandle2.Init("_TEMP_MATERIALINJECTOR_TARGET_2");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {

            try
            {

                CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
                //1.copiar y pegar la textura pantalla
                RenderTextureDescriptor camaraDescriptor = renderingData.cameraData.cameraTargetDescriptor;
                //referencia de la  textura de la pantalla 
                RenderTargetIdentifier cameraTargetId = renderingData.cameraData.renderer.cameraColorTarget;
                //1.3 textura de donde vamos a copiar 
                cmd.GetTemporaryRT(temporaryTextureHandle.id, camaraDescriptor);
                cmd.GetTemporaryRT(temporaryTextureHandle2.id, camaraDescriptor);
                //loop intercambio 

                RenderTargetIdentifier a = cameraTargetId;
                RenderTargetIdentifier b = temporaryTextureHandle.Identifier();


                //1.4 COPIAR TEXTURA 
                cmd.Blit(cameraTargetId, temporaryTextureHandle.Identifier());
                a = temporaryTextureHandle.Identifier();
                b = temporaryTextureHandle2.Identifier();

               


                //2 modificar la textura 
                //encontrar el material de modificacion 
                VolumeStack volumeStack = VolumeManager.instance.stack;
                MaterialInjector materialInjector = volumeStack.GetComponent<MaterialInjector>();
                Material material = materialInjector.material.value;
                for (int i = 0; i < materialInjector.iteraction.value; i++)
                {
                    cmd.Blit(a,b, material, material.FindPass("Universal Forward"));
                    (a, b) = (b, a);
                }
                a = b;
                b = cameraTargetId;
                cmd.Blit(a,b);
                //modificar a la pandatta
                cmd.Blit(temporaryTextureHandle.Identifier(), cameraTargetId, material,
                    material.FindPass("Universal Forward"));
                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
                executeSucces = true;
            }
            catch (Exception e)
            {
                executeSucces = false;
            }
        }  


    // Cleanup any allocated resources that were created during the execution of this render pass.
    public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (executeSucces)
            {
                cmd.ReleaseTemporaryRT(temporaryTextureHandle.id); 
                
            }
           
        }
    }

    CustomRenderPass m_ScriptablePass;
    public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRendering;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(name);

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = renderPassEvent;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}



