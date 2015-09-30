using System;

public class VMAttachmentSocketOverride : ViewModelAttachment
{
	public Socket.CameraSpace socketOverride;

	public VMAttachmentSocketOverride()
	{
	}

	private void OnDrawGizmosSelected()
	{
		this.socketOverride.DrawGizmos("socketOverride");
	}
}