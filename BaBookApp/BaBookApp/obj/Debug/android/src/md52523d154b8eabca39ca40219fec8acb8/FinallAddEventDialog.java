package md52523d154b8eabca39ca40219fec8acb8;


public class FinallAddEventDialog
	extends android.app.DialogFragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onAttach:(Landroid/app/Activity;)V:GetOnAttach_Landroid_app_Activity_Handler\n" +
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("BaBookApp.Resources.Fragments.Dialog.FinallAddEventDialog, BaBookApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", FinallAddEventDialog.class, __md_methods);
	}


	public FinallAddEventDialog ()
	{
		super ();
		if (getClass () == FinallAddEventDialog.class)
			mono.android.TypeManager.Activate ("BaBookApp.Resources.Fragments.Dialog.FinallAddEventDialog, BaBookApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onAttach (android.app.Activity p0)
	{
		n_onAttach (p0);
	}

	private native void n_onAttach (android.app.Activity p0);


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public android.view.View onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2)
	{
		return n_onCreateView (p0, p1, p2);
	}

	private native android.view.View n_onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}