using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "CharacterInfo", menuName = "CreateCharacterInfo")]
public class CharacterInfo : ScriptableObject
{
	// キャラクター情報
	// 識別子
	[SerializeField]
	public string id;
	// 名前
	[SerializeField]
    public new string name;
	// 別名
	[SerializeField]
	public string alias;
	// 説明
	[SerializeField]
	public string detail;
	// タイプ atack/defence/support
	[SerializeField]
	[HideInInspector]
	public string type;
	// 画像素材
	[SerializeField]
	public Image image;
	// レア度 1:common 2:uncommon 3:rare
	[SerializeField]
	public int rank;
	// ステータス
	[SerializeField]
	public List<Status> status = new List<Status>(10);
	// スキル
	[SerializeField]
	public Skill skill;
	// 奥義
	[SerializeField]
	public Special special;
	// 購入金額
	[SerializeField]
	public int price;

	// ステータス情報
	[System.SerializableAttribute]
	public class Status
	{
		// 召喚コスト
		public int cost;
		// 攻撃力
		public int attack;
		// 攻撃速度
		[HideInInspector]
		public float interval;
		// HP
		public int hp;
		// 移動速度
		public float speed;
		// 与ノックバック割合
		public float atkKB;
		// 被ノックバック割合
		public float defKB;
		// レベルアップコスト
		public int growth;
	}

	// スキル
	[System.SerializableAttribute]
	public class Skill
	{
		// スキル名
		public string name;
		// 消費魔力
		public int cost;
		// クールダウン
		public float cd;
		// 説明
		public string Detail;
		// 召喚エフェクト
		[HideInInspector]
		public GameObject effect;
		// 効果割合(エフェクトなしで発動するキャラの場合)
		public float Ratio;
	}

	// 奥義
	[System.SerializableAttribute]
	public class Special
	{
		// 奥義名
		public string name;
		// 消費魔力
		public int cost;
		// クールダウン
		public float cd;
		// 説明
		public string Detail;
		// 召喚エフェクト
		[HideInInspector]
		public GameObject effect;
		// 効果割合(エフェクトなしで発動するキャラの場合)
		public float Ratio;
		// 演出プレハブ
		[HideInInspector]
		public GameObject still;
		// 背景効果
		[HideInInspector]
		public Sprite BackGround;
	}

	// キャラクター画像関連
	[System.SerializableAttribute]
	public class Image
	{
		// 全身画像
		public Sprite fullsize;
		// 背景画像
		public Sprite backGround;
		// 重ね効果
		public Sprite effect;
		// アイコン
		public Sprite icon;
		// バトルキャラクター
		public GameObject prefab;
	}
}
