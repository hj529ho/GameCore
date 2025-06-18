using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Define
{
    #region Enum
    public enum UIEvent
    {
        Click,
        Drag,
        Drop,
        PointUp,
        PointDown,
        PointEnter,
        PointExit,
    }
    public enum Sound
    {
        BGM,
        SFX,
        MaxCOUNT
    }
    public enum ObjectType
    {
        Player,
        Monster,
        Projectile,
    }
    public enum ToastType
    {
        Top,
        Bottom

    }
    public enum HTTP_TYPE
    {
        Get,
        Post,
        Put,
        Delete,
        Patch
    }
    public enum LoadType
    {
        Global,
        Stage
    }
    public enum Story
    {
        story1,
    }
    public enum StoryUIOder
    {
        UI_Test,
        UI_Dialog,
        MaxCount
    }

    public enum SkillType
    {
        None,
        ArrowSkill = 100001,
        BulletSkill = 100011,
    }

    public enum PassiveSkillType
    {
        None,
        CoolTimeDown = 200001,
    }

    public enum PatchState
    {
        None,
        Init,
        Check,
        SizeCheck,
        NeedUpdate,
        Download,
        Done,
        Skip
    }
    public enum GridStatus
    {
        Empty =0,
        Filled=1,
        Pivot=2
    }
    public enum CellStatus
    {
        Empty =0,
        Filled = 1
    }
    public enum Grade
    {
        None =0,
        Common =1,
        Uncommon =2,
        Rare=3,
        Epic=4,
        Legendary=5
    }
    
    public enum StickerTarget
    {
        None,
        AttackSticker,
        Player,
    }
    
    
    public enum StickerEffect
    {
        AttackPower,
        FinalDamage,
        AttackSpeed,
        MoveSpeed,
        CriticalRate,
        CriticalDamage,
        Health,
        Armor,
        Dodge,
        Recovery,
        NumberOfProjectile,
        KnockBack,
        Explosion,
        LifeSteal,
        Pierce,
        ProjectileScale,
        Experience,
        Discount,
    }
    
    public enum StickerCondition
    {
        None,
        AdjacentTo,
        AdjacentWall,
        NotAdjacentWall,
        MoreThan,
        LessThan,
        //특정 종류의 스티커가 n개 이상일 경우
    }
    public enum StickerType
    {
        Normal,
        Attack,
    }
    public enum StickerOwner
    {
        Player,
        Store,
    }
    public enum StickerAnimationState
    {
        Idle,
        Drag,
        Possible,
        Error,
    }

    public enum GameMode
    {
        TrainHard,
        FantasticBaseball,
        /// <summary>
        /// Like mario
        /// </summary>
        SuperJump,
        NinjaMaster,
    }
    public enum GameState
    {
        PreRound,
        StartGame,
        PostRound,
        None,
    }
    /// <summary>
    /// 게임 결과
    /// </summary>
    public enum GameResult
    {
        None,

        Win,
        Lose,
        // Tie,
    }
    /// <summary>
    /// 게임 종료 사유
    /// </summary>
    public enum GameDetailResult
    {
        Normal,
        Death,
    }

    public enum BorderSides : int
    {
        Top = 0,
        Bottom,
        Left,
        Right,
        Count,
    }

    [Serializable]
    public enum Equipments : int
    {
        Head = 0b0001,
        Hand = 0b0010,
        Chest = 0b0100,
        Foot = 0b1000,
    }

    public enum CharacterRanks
    {
        B = 0,
        A = 1,
        S = 2,
    }
    #endregion

    #region Delegate
    public delegate void OnInitDel();
    #endregion
}
