package com.unity3d.player;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.ActivityInfo;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.webkit.WebResourceError;
import android.webkit.WebResourceRequest;
import android.webkit.WebView;
import android.webkit.WebViewClient;
 
public class PrivacyActivity extends Activity implements DialogInterface.OnClickListener {
    boolean useLocalHtml = true;
    String privacyUrl = "你的隐私政策地址";
    final String htmlStr = "欢迎使用本游戏，在使用本游戏前，请您充分阅读并理解 <a href=\"https://note.youdao.com/s/QlsrqI9i\">" +
                                        "《Unity隐私政策》</a>和<a href=\"https://developer.taptap.com/docs/sdk/start/agreement/\">《TapTap隐私政策》</a>各条;\n" +
                                "1.保护用户隐私是本游戏的一项基本政策，本游戏不会泄露您的个人信息；\n\n" +
                                "2.我们会根据您使用的具体功能需要，收集必要的用户信息（如申请设备信息，存储等相关权限）；\n\n" +
                                "3.在您同意App隐私政策后，我们将进行集成SDK的初始化工作，会收集您的android_id、Mac地址、IMEI和应用安装列表，以保障App正常数据统计和安全风控；\n\n" +
                                "4.您可以阅读完整版的隐私保护政策了解我们申请使用相关权限的情况，以及对您个人隐私的保护措施；\n\n" +
                                "5.为了保障游戏安全稳定运行并实现特定功能，会接入第三方开发的软件工具开发包，我们会对游戏接入的涉及个人信息收集的SDK进行安全监测，以保护您的个人信息安全：\n\n"+
                                "5.1 SDK名称：Unity 3D\n"+
                                "5.1.1 SDK使用目的：框架\n"+
                                "5.1.2 SDK收集的信息：Android ID、设备传感器信息\n";;
                       
 
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
 
        ActivityInfo actInfo = null;
        try {
            //获取AndroidManifest.xml配置的元数据
            actInfo = this.getPackageManager().getActivityInfo(getComponentName(), PackageManager.GET_META_DATA);
            useLocalHtml = actInfo.metaData.getBoolean("useLocalHtml");
            privacyUrl = actInfo.metaData.getString("privacyUrl");
        } catch (PackageManager.NameNotFoundException e) {
            e.printStackTrace();
        }
 
        //如果已经同意过隐私协议则直接进入Unity Activity
        if (GetPrivacyAccept()){
            EnterUnityActivity();
            return;
        }
        ShowPrivacyDialog();//弹出隐私协议对话框
    }
 
    @Override
    public void onClick(DialogInterface dialogInterface, int i) {
        switch (i){
            case AlertDialog.BUTTON_POSITIVE://点击同意按钮
                SetPrivacyAccept(true);
                EnterUnityActivity();//启动Unity Activity
                break;
            case AlertDialog.BUTTON_NEGATIVE://点击拒绝按钮,直接退出App
                finish();
                break;
        }
    }
    private void ShowPrivacyDialog(){
        WebView webView = new WebView(this);
        webView.loadDataWithBaseURL(null, htmlStr, "text/html", "UTF-8", null);

        AlertDialog.Builder privacyDialog = new AlertDialog.Builder(this);
        privacyDialog.setCancelable(false);
        privacyDialog.setView(webView);
        privacyDialog.setTitle("用户协议及隐私协议");
        privacyDialog.setNegativeButton("退出",this);
        privacyDialog.setPositiveButton("同意",this);
        privacyDialog.create().show();
    }
//启动Unity Activity
    private void EnterUnityActivity(){
        Intent unityAct = new Intent();
        unityAct.setClassName(this, "com.unity3d.player.UnityPlayerActivity");
        this.startActivity(unityAct);
    }
//保存同意隐私协议状态
    private void SetPrivacyAccept(boolean accepted){
        SharedPreferences.Editor prefs = this.getSharedPreferences("PlayerPrefs", MODE_PRIVATE).edit();
        prefs.putBoolean("PrivacyAccepted", accepted);
        prefs.apply();
    }
    private boolean GetPrivacyAccept(){
        SharedPreferences prefs = this.getSharedPreferences("PlayerPrefs", MODE_PRIVATE);
        return prefs.getBoolean("PrivacyAccepted", false);
    }
}