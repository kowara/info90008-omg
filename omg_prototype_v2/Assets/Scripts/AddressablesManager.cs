/*////////////////////////////////////////////////////////////
 * INFO90008 - HCI Project
 * Addressing challenges in the digitisation of board games:
 * digitising the core functions of Oh My Goods!
 * 
 * developed by Patricia Amanda Kowara (PKOWARA/1028290)
 * supervised by Dr. Melissa Rogerson
 ///////////////////////////////////////////////////////////*/

/*////////////////////////////////////////////////////////////////////
 * AddressablesManager.cs
 * Manages all game Addressables...or is supposed to.
 * Current version doesn't use Addressables yet.
 * For now, this script functions as Sprite and GameObject containers.
 ///////////////////////////////////////////////////////////////////*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesManager : MonoBehaviour
{
    //public Sprite sprite;
    public Sprite spriteStone;
    public Sprite spriteCotton;
    public Sprite spriteWood;
    public Sprite spriteClay;
    public Sprite spriteWheat;
    public Sprite spriteWorker;
    public Sprite spriteCharburner;
    public Sprite spriteClosed;
    public Sprite halfSun;
    public Sprite product;
    public Sprite factoryWorker;
    public GameObject cardOutline;
    public GameObject button;
    public GameObject inProduction;
    public Font font;

    //Addressables setup for future iteration
    //[SerializeField] AssetReferenceSprite assetReferenceSpriteStone;
    //[SerializeField] AssetReferenceSprite assetReferenceSpriteCotton;
    //[SerializeField] AssetReferenceSprite assetReferenceSpriteWood;
    //[SerializeField] AssetReferenceSprite assetReferenceSpriteClay;
    //[SerializeField] AssetReferenceSprite assetReferenceSpriteWheat;

    //[SerializeField] AssetReferenceSprite assetReferenceSpriteCharburner;
    //[SerializeField] AssetReferenceSprite assetReferenceSpriteWorker;

    //public void AddressablesSpriteStone()
    //{
    //    assetReferenceSpriteStone.LoadAssetAsync().Completed += OnSpriteLoaded;
    //}
    //public void AddressablesSpriteCotton()
    //{
    //    assetReferenceSpriteCotton.LoadAssetAsync().Completed += OnSpriteLoaded;
    //}
    //public void AddressablesSpriteWood()
    //{
    //    assetReferenceSpriteWood.LoadAssetAsync().Completed += OnSpriteLoaded;
    //}
    //public void AddressablesSpriteClay()
    //{
    //    assetReferenceSpriteClay.LoadAssetAsync().Completed += OnSpriteLoaded;
    //}
    //public void AddressablesSpriteWheat()
    //{
    //    assetReferenceSpriteWheat.LoadAssetAsync().Completed += OnSpriteLoaded;
    //}
    //public void AddressablesSpriteCharburner()
    //{
    //    assetReferenceSpriteCharburner.LoadAssetAsync().Completed += OnSpriteLoaded;
    //}
    //public void AddressablesSpriteWorker()
    //{
    //    assetReferenceSpriteWorker.LoadAssetAsync().Completed += OnSpriteLoaded;
    //}

    //void OnSpriteLoaded(AsyncOperationHandle<Sprite> handle)
    //{
    //    sprite = handle.Result;
    //    Debug.Log(sprite.name);
    //}
}
