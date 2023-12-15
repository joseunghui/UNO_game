using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UI_LoadingVideo : UI_Scene
{

    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();

        Bind<GameObject>(typeof(Define.LoadgingVideo));
        Bind<RawImage>(typeof(Define.LoadgingVideo));
        Bind<VideoPlayer>(typeof(Define.LoadgingVideo));
        Bind<Image>(typeof(Define.LoadgingVideo));

        GetGameObject((int)Define.LoadgingVideo.UI_LoadingVideo);
        Get<RawImage>((int)Define.LoadgingVideo.LoadingImage);
        Get<VideoPlayer>((int)Define.LoadgingVideo.LoadingVideo);
        GetImage((int)Define.LoadgingVideo.LoadingBar);
    }
}
