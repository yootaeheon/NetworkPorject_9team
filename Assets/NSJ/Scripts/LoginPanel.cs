using Firebase.Extensions;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BaseUI
{
    #region private 필드

    private Color _defaultInputColor;
    enum Box { Login, Find, SignUp, SendSuccess, SendFail, ConfirmSend, Size }
    private GameObject[] _boxs = new GameObject[(int)Box.Size];
    // 로그인 박스
    private TMP_InputField _loginEmailInput;
    private TMP_InputField _loginPasswordInput;
    private GameObject _loginButtonOn;

    // 회원 가입
    private TMP_InputField _signUpEmailInput;
    private TMP_InputField _signUpNickNameInput;
    private TMP_InputField _signUp1stNameInput;
    private TMP_InputField _signUp2ndNameInput;
    private TMP_InputField _signUpPasswordInput;
    private TMP_InputField _signUpConfirmInput;
    private GameObject _signUpButtonOn;

    // 비밀번호 찾기 
    private TMP_InputField _findEmailInput;
    private GameObject _findButtonOn;
    #endregion

    private void Awake()
    {
        Bind(); // 바인딩
        Init(); // 초기 설정
        SubscribeEvent(); // 이벤트 구독
    }

    private void OnEnable()
    {
        ChangeBox(Box.Login);
    }
    #region 로그인
    /// <summary>
    /// 로그인
    /// </summary>
    private void Login()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    #endregion
    #region 회원 가입
    private void ActivateSignUpButton(string value)
    {
        _signUpButtonOn.SetActive(false);
        // 모든 InputField 를 작성해야만 로그인 버튼 활성화
        if (_signUpEmailInput.text == string.Empty) 
            return;
        if(_signUpPasswordInput.text == string.Empty) 
            return;
        if (_signUpConfirmInput.text == string.Empty)
            return;
        if (_signUpNickNameInput.text == string.Empty)
            return;
        if (_signUp1stNameInput.text == string.Empty)
            return;
        if (_signUp2ndNameInput.text == string.Empty)
            return;
        _signUpButtonOn.SetActive(true);
    }

    /// <summary>
    /// 회원 가입
    /// </summary>
    private void SignUp()
    {
        string email = _signUpEmailInput.text;
        string password = _signUpPasswordInput.text;
        string confirm = _signUpConfirmInput.text;
        if(password != confirm)
        {
            _signUpPasswordInput.text = string.Empty;
            _signUpPasswordInput.placeholder.color = Util.GetColor(Color.red, _defaultInputColor.a);
            _signUpConfirmInput.text = string.Empty;
            _signUpConfirmInput.placeholder.color = Util.GetColor(Color.red, _defaultInputColor.a);
        }

        BackendManager.Auth.CreateUserWithEmailAndPasswordAsync(email, password).
            ContinueWithOnMainThread(task =>
            {

            });
    }
    #endregion

    #region 패널 조작
    /// <summary>
    ///  UI 박스 변경
    /// </summary>
    private void ChangeBox(Box box)
    {
        for (int i = 0; i < _boxs.Length; i++)
        {
            if (i == (int)box) // 선택한 박스 빼고 초기화
            {
                _boxs[i].SetActive(true);
                ClearBox(box); //  선택한 해당박스는 초기화 
            }
            else
            {
                _boxs[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 로그인 패널 박스 클리어 선택
    /// </summary>
    private void ClearBox(Box box)
    {
        switch (box)
        {
            case Box.Login:
                ClearLoginBox();
                break;
            case Box.SignUp:
                ClearSignUpBox();
                break;
            case Box.Find:
                ClearFindBox();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 로그인 박스 초기화
    /// </summary>
    private void ClearLoginBox()
    {
        _loginEmailInput.text = string.Empty;
        _loginPasswordInput.text = string.Empty;
        _loginButtonOn.SetActive(false);
    }

    /// <summary>
    /// 회원가입 박스 초기화
    /// </summary>
    private void ClearSignUpBox()
    {
        _signUpEmailInput.text = string.Empty;
        _signUpNickNameInput.text = string.Empty;
        _signUpPasswordInput.text = string.Empty;
        _signUpPasswordInput.placeholder.color = _defaultInputColor;
        _signUpConfirmInput.text = string.Empty;
        _signUpConfirmInput.placeholder.color = _defaultInputColor;
        _signUp1stNameInput.text = string.Empty;
        _signUp2ndNameInput.text = string.Empty;
        _signUpButtonOn.SetActive(false);
    }

    /// <summary>
    /// 비밀번호 찾기 박스 초기화
    /// </summary>
    private void ClearFindBox()
    {
        _findEmailInput.text = string.Empty;
        _findButtonOn.SetActive(false);
    }
    #endregion
    #region 초기 설정
    /// <summary>
    /// 초기 설정
    /// </summary>
    private void Init()
    {
        #region Box 배열 설정
        _boxs[(int)Box.Login] = GetUI("LoginBox");
        _boxs[(int)Box.Find] = GetUI("FindBox");
        _boxs[(int)Box.SignUp] = GetUI("SignUpBox");
        _boxs[(int)Box.SendSuccess] = GetUI("SendSuccessBox");
        _boxs[(int)Box.SendFail] = GetUI("SendFailBox");
        _boxs[(int)Box.ConfirmSend] = GetUI("ConfirmSendBox");
        #endregion
        #region LoginBox
        _loginEmailInput = GetUI<TMP_InputField>("LoginEmailInput");
        _loginPasswordInput = GetUI<TMP_InputField>("LoginPasswordInput");
        _loginButtonOn = GetUI("LoginButton");
        #endregion
        #region SignUpBox
        _signUpEmailInput = GetUI<TMP_InputField>("SignUpEmailInput");
        _signUpNickNameInput = GetUI<TMP_InputField>("SignUpNickNameInput");
        _signUp1stNameInput = GetUI<TMP_InputField>("SignUp1stNameInput");
        _signUp2ndNameInput = GetUI<TMP_InputField>("SignUp2ndNameInput");
        _signUpPasswordInput = GetUI<TMP_InputField>("SignUpPasswordInput");
        _signUpConfirmInput = GetUI<TMP_InputField>("SignUpConfirmInput");
        _signUpButtonOn = GetUI("SignUpButton");
        #endregion
        #region FindBox
        _findEmailInput = GetUI<TMP_InputField>("FindEmailInput");
        _findButtonOn = GetUI("FindButton");
        #endregion

        _defaultInputColor = _loginEmailInput.placeholder.color;
    }
    /// <summary>
    /// 이벤트 구독
    /// </summary>
    private void SubscribeEvent()
    {
        #region LoginBox
        GetUI<Button>("LoginFindButton").onClick.AddListener(() => ChangeBox(Box.Find)); // 비밀번호 찾기 버튼
        GetUI<Button>("LoginSignUpButton").onClick.AddListener(() => ChangeBox(Box.SignUp)); // 회원가입 버튼
        GetUI<Button>("LoginButton").onClick.AddListener(Login);
        #endregion
        #region SignUpBox
        GetUI<Button>("SignUpButton").onClick.AddListener(SignUp);
        GetUI<Button>("SignUpBackButton").onClick.AddListener(() => ChangeBox(Box.Login));
        _signUpEmailInput.onValueChanged.AddListener(ActivateSignUpButton);
        _signUpPasswordInput.onValueChanged.AddListener(ActivateSignUpButton);
        _signUpConfirmInput.onValueChanged.AddListener(ActivateSignUpButton);
        _signUpNickNameInput.onValueChanged.AddListener(ActivateSignUpButton);
        _signUp1stNameInput.onValueChanged.AddListener(ActivateSignUpButton);
        _signUp2ndNameInput.onValueChanged.AddListener(ActivateSignUpButton);
        #endregion
        #region FindBox
        GetUI<Button>("FindBackButton").onClick.AddListener(() => ChangeBox(Box.Login));
        GetUI<Button>("FindButton").onClick.AddListener(() => {/* TODO : 비밀번호 찾기 메서드 */});
        #endregion
        GetUI<Button>("QuitButton").onClick.AddListener(() =>  // 종료 버튼
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
             Application.Quit();
#endif
        });
    }
    #endregion
}
