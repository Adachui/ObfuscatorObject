using UnityEngine;
using System.Collections;

/// <summary>
/// An helper to avoid duplicate code
/// </summary>
public class AppAdvisoryHelper : MonoBehaviour
{
	private PlaneGameManager _planeGameManager;
	public PlaneGameManager planeGameManager
    {
		get
		{
			if(_planeGameManager == null)
                _planeGameManager = FindObjectOfType<PlaneGameManager>();

			return _planeGameManager;
		}
	}

	private BlocksManager _blocksManager;
	public BlocksManager blocksManager
    {
		get
		{
			if(_blocksManager == null)
                _blocksManager = FindObjectOfType<BlocksManager>();

			return _blocksManager;
		}
	}

	//private AudioSource _audioSource;
	//public AudioSource audioSource
	//{
	//	get
	//	{
	//		if(_audioSource == null)
	//			_audioSource = FindObjectOfType<AudioSource>();

	//		return _audioSource;
	//	}
	//}

    private CanvasManager _canvasManager;
    public CanvasManager canvasManager
    {
        get
        {
            if (_canvasManager == null)
                _canvasManager = FindObjectOfType<CanvasManager>();

            return _canvasManager;
        }
    }

	private UserInfoView _userInfoView;
	public UserInfoView userInfoView
    {
        get
        {
			if (_userInfoView == null)
				_userInfoView = FindObjectOfType<UserInfoView>();

			return _userInfoView;
        }
    }

	private ShopManager _shopManager;
	public ShopManager shopManager
	{
		get
		{
			if(_shopManager == null)
				_shopManager = FindObjectOfType<ShopManager>();

			return _shopManager;
		}
	}

    public  virtual void InitModelData(IModel _model)
    {

    }


	private SoundManager _soundManager;
	public SoundManager soundManager
	{
		get
		{
			if(_soundManager == null)
				_soundManager = FindObjectOfType<SoundManager>();

			return _soundManager;
		}
	}

	private Guide _guideLayer;
	public Guide guideLayer
	{
		get
		{
			if(_guideLayer == null)
				_guideLayer = FindObjectOfType<Guide>();

			return _guideLayer;
		}
	}

	private MonsterPlaneMgr _monsterPlaneMgr;
	public MonsterPlaneMgr monsterPlaneMgr
	{
		get
		{
			if(_monsterPlaneMgr == null)
				_monsterPlaneMgr = FindObjectOfType<MonsterPlaneMgr>();

			return _monsterPlaneMgr;
		}
	}

	private AttackMgr _attackMgr;
	public AttackMgr attackMgr
	{
		get
		{
			if(_attackMgr == null)
				_attackMgr = FindObjectOfType<AttackMgr>();

			return _attackMgr;
		}
	}

	private DefendPlaneMgr _defendPlaneMgr;
	public DefendPlaneMgr defendPlaneMgr
	{
		get
		{
			if(_defendPlaneMgr == null)
				_defendPlaneMgr = FindObjectOfType<DefendPlaneMgr>();

			return _defendPlaneMgr;
		}
	}
}
