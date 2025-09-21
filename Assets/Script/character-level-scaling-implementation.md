# キャラクターレベルに応じた大きさ変更の実装方針

## 概要
キャラクターのレベルに応じて、キャラクターの表示サイズを変更する機能を実装する。

## 現状分析

### 既存のレベルシステム
- キャラクターレベル: 0-4の5段階（CharacterCore.csで管理）
- レベルデータはSaveControllerで永続化
- レベルアップはCharacterMenuController.csで実装済み

### 既存のサイズ変更関連実装
- CharacterCore.csでtransform.localScaleは向きの制御にのみ使用（line 390）
- キャラクターのサイズ変更に関する既存実装はなし

## 実装方針

### 1. データ構造の拡張

#### CharacterInfo（ScriptableObject）への追加
```csharp
[System.Serializable]
public class StatusData
{
    public float hp;
    public float attack;
    public float speed;
    public float atkKB;
    public float defKB;
    public int growth;
    public int cost;
    public float scale = 1.0f; // 新規追加：レベルごとのスケール値
}
```

### 2. CharacterCore.csの修正

#### SetCharacterInfo()メソッドの拡張
```csharp
public void SetCharacterInfo(int level)
{
    characterInfo = Resources.Load<CharacterInfo>($"{CHARACTER_INFO_PATH}{characterId}");
    
    // 既存のステータス設定
    maxHp = characterInfo.status[level].hp;
    Hp = maxHp;
    maxSpeed = characterInfo.status[level].speed / 20;
    Speed = maxSpeed;
    atkPower = characterInfo.status[level].attack;
    atkKB = characterInfo.status[level].atkKB;
    defKB = characterInfo.status[level].defKB;
    
    // 新規追加：レベルに応じたスケール設定
    ApplyLevelScale(characterInfo.status[level].scale);
}

private void ApplyLevelScale(float scale)
{
    // 現在の向きを保持したままスケールを適用
    float currentDirection = Mathf.Sign(transform.localScale.x);
    transform.localScale = new Vector3(
        scale * currentDirection,
        scale,
        1f
    );
}
```

### 3. スケール値の設計

#### 推奨されるスケール値
- レベル0: 0.8f（小さめ）
- レベル1: 0.9f
- レベル2: 1.0f（標準サイズ）
- レベル3: 1.1f
- レベル4: 1.2f（最大サイズ）

### 4. 影響範囲と考慮事項

#### 当たり判定への影響
- BoxCollider2Dのサイズもスケールに応じて自動的に変更される
- 攻撃範囲、被ダメージ判定も比例して変化

#### 位置調整
- キャラクターの足元位置（pivot）を基準にスケーリング
- 地面との接地を維持するため、必要に応じてY座標の調整が必要

#### バランス調整
- 大きいキャラクターは当たりやすくなるデメリット
- 攻撃範囲が広がるメリット
- ゲームバランスを考慮した調整が必要

### 5. 実装手順

1. **CharacterInfoのScriptableObject拡張**
   - StatusDataにscaleフィールドを追加
   - 既存のキャラクターデータにデフォルト値（1.0f）を設定

2. **CharacterCore.csの修正**
   - SetCharacterInfo()メソッドにスケール適用処理を追加
   - ApplyLevelScale()メソッドの実装

3. **CSVデータの更新**
   - CSVLoaderCharacter.csでscale列の読み込み処理を追加
   - キャラクターCSVファイルにscale列を追加

4. **テストと調整**
   - 各キャラクターでレベル0-4のサイズ変化を確認
   - 当たり判定の適切性を検証
   - ゲームバランスの調整

### 6. オプション機能

#### アニメーション対応
- レベルアップ時のスケール変化アニメーション
- DOTweenを使用したスムーズな拡大/縮小演出

#### UI表示
- キャラクターメニューでのプレビュー表示
- レベルアップ時のサイズ変化プレビュー

## まとめ
この実装により、キャラクターのレベルが視覚的にも分かりやすくなり、ゲームプレイの戦略性も向上する。実装は既存システムとの親和性を保ちながら、最小限の変更で実現可能。