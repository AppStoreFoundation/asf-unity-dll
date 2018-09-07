using UnityEngine;
using UnityEngine.Events;

namespace Aptoide.AppcoinsUnity
{
    public class MessageHandlerGUI : MonoBehaviour
    {
        public class PropagateSelection : UnityEvent<bool> {}
        private Rect windowRect;
        
        private GUIContent title;
        private string message;
        private string success;
        private string fail;
        
        private float windowHeight;
        private float windowWidth;

        private bool isEnabled = false;
        public PropagateSelection prop;

        Canvas canvas;

        private void Awake()
        {
            prop = new PropagateSelection();

            canvas = (Canvas) FindObjectOfType<Canvas>();

            float screenHeight = Screen.height;
            float screenWidth = Screen.width;
            
            // Make MessageHandlerGUI window with height of 40% of the screen's
            // height and 40% of screen's width
            windowWidth = (0.4F * screenWidth);
            windowHeight = (0.4F * screenHeight);
            
            // Center Window
            CenterWindow(windowWidth, windowHeight, screenWidth, screenHeight, 
                         out float x, out float y);
            
            windowRect = new Rect(x, y, windowWidth, windowHeight);
        }
        
        private void CenterWindow(float x, float y, float outerWidth, 
                                  float outerHeight, out float newX, 
                                  out float newY)
        {
            newX = Mathf.Abs((outerWidth - x) * 0.5F);
            newY = Mathf.Abs((outerHeight - y) * 0.5F);
        }
        
        private void OnGUI()
        {
            if (isEnabled)
            {
                canvas.enabled = false;

                // Register window
                windowRect = GUI.ModalWindow(0, windowRect, DoGUI, title);
            }

            canvas.enabled = true;

        }

        private void DoGUI(int windowId)
        {
            float height = 20F;

            // Message Area
            GUI.Label(
                new Rect(
                    15, 
                    height, 
                    windowWidth - 15, 
                    (height += 0.6F * windowHeight)
                ), 
                message, 
                GUI.skin.textArea
            );
            
            // Success Button (Position: Bottom right corner; 
            //  Heigth: 10% of the window's heigth; Width: 20% of the window's 
            //  width
            float buttonWidthScale = 0.3F;
            float buttonHeightScale = 0.1F;
            float deltaX = 0.05F * windowWidth;  // 5% of margin
            float delatY = 0.05F * windowHeight;  // 5% of margin
            
            float buttonWidth = buttonWidthScale * windowWidth;
            float buttonHeight = buttonHeightScale * windowHeight;
            float successButtonX = windowWidth - deltaX - buttonWidth;
            float successButtonY = windowHeight - delatY - buttonHeight;
            
            if (GUI.Button(
                    new Rect(
                        successButtonX, 
                        successButtonY, 
                        buttonWidth, 
                        buttonHeight
                    ), 
                    success
                )
               )
            {
                isEnabled = false;
                prop.Invoke(true);
            }
            
            if (fail != null)
            {
                // Fail Button (Position: Same height of the succes button and 
                // it is at the left of the success button; Heigth: 10% of the 
                // window's heigth; Width: 20% of the window's width
                float failButtonX = successButtonX - deltaX - buttonWidth;
                float failButtonY = successButtonY;
                
                if (GUI.Button(
                        new Rect(
                            failButtonX, 
                            failButtonY, 
                            buttonWidth, 
                            buttonHeight
                        ), 
                        fail
                    )
                   )
                {
                    isEnabled = false;
                    prop.Invoke(false);
                }
            }
        }
        
        public void ChangeContent(string m, string s, string f)
        {
            message = m;
            success = s;
            fail = f;
        }
        
        public void ChangeTitle(string t)
        {
            title = new GUIContent(t);
        }

        public void Enable()
        {
            isEnabled = true;
        }
    }
}