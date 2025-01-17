using UnityEngine;

namespace NVC
{
    /// <summary>
    /// Transform プロパティをキャッシュする MonoBehaviour
    /// </summary>
    public abstract class CacheableMonoBehaviour : MonoBehaviour
    {
        //================================================================================
        // 変数
        //================================================================================
        private Transform m_TransformCache; // transform プロパティのキャッシュ

        //================================================================================
        // プロパティ
        //================================================================================
        /// <summary>
        /// Transform を返します
        /// </summary>
        public Transform Transform
        {
            get
            {
                if ( m_TransformCache == null )
                {
                    m_TransformCache = base.transform;
                }

                return m_TransformCache;
            }
        }

        /// <summary>
        /// ワールド座標における位置を取得または設定します
        /// </summary>
        public Vector3 Position { get => Transform.position; set => Transform.position = value; }

        /// <summary>
        /// ワールド座標における回転角を取得または設定します
        /// </summary>
        public Quaternion Rotation { get => Transform.rotation; set => Transform.rotation = value; }

        /// <summary>
        /// ワールド座標における回転角を取得または設定します
        /// </summary>
        public Vector3 EulerAngles { get => Transform.eulerAngles; set => Transform.eulerAngles = value; }

        /// <summary>
        /// ローカル座標における位置を取得または設定します
        /// </summary>
        public Vector3 LocalPosition { get => Transform.localPosition; set => Transform.localPosition = value; }

        /// <summary>
        /// ローカル座標における回転角を取得または設定します
        /// </summary>
        public Quaternion LocalRotation { get => Transform.localRotation; set => Transform.localRotation = value; }

        /// <summary>
        /// ローカル座標における回転角を取得または設定します
        /// </summary>
        public Vector3 LocalEulerAngles { get => Transform.localEulerAngles; set => Transform.localEulerAngles = value; }

        /// <summary>
        /// ローカル座標におけるスケーリング値を取得または設定します
        /// </summary>
        public Vector3 LocalScale { get => Transform.localScale; set => Transform.localScale = value; }

        //================================================================================
        // ワールド座標における位置
        //================================================================================
        /// <summary>
        /// ワールド座標における位置をリセットします
        /// </summary>
        public void ResetPosition()
        {
            Transform.position = Vector3.zero;
        }

        /// <summary>
        /// ワールド座標における X 座標を設定します
        /// </summary>
        public void SetPositionX( float x )
        {
            var pos = Transform.position;
            pos = new
            (
                x,
                pos.y,
                pos.z
            );
            Transform.position = pos;
        }

        /// <summary>
        /// ワールド座標における Y 座標を設定します
        /// </summary>
        public void SetPositionY( float y )
        {
            var pos = Transform.position;
            pos = new
            (
                pos.x,
                y,
                pos.z
            );
            Transform.position = pos;
        }

        /// <summary>
        /// ワールド座標における Z 座標を設定します
        /// </summary>
        public void SetPositionZ( float z )
        {
            var pos = Transform.position;
            pos = new
            (
                pos.x,
                pos.y,
                z
            );
            Transform.position = pos;
        }

        /// <summary>
        /// Vector2 型でワールド座標における位置を設定します
        /// </summary>
        public void SetPosition( Vector2 v )
        {
            Transform.position = new
            (
                v.x,
                v.y,
                Transform.position.z
            );
        }

        /// <summary>
        /// Vector3 型でワールド座標における位置を設定します
        /// </summary>
        public void SetPosition( Vector3 v )
        {
            Transform.position = v;
        }

        /// <summary>
        /// ワールド座標における位置を設定します
        /// </summary>
        public void SetPosition( float x, float y )
        {
            Transform.position = new
            (
                x,
                y,
                Transform.position.z
            );
        }

        /// <summary>
        /// ワールド座標における位置を設定します
        /// </summary>
        public void SetPosition( float x, float y, float z )
        {
            Transform.position = new
            (
                x,
                y,
                z
            );
        }

        /// <summary>
        /// ワールド座標における X 座標に加算します
        /// </summary>
        public void AddPositionX( float x )
        {
            var pos = Transform.position;
            pos = new
            (
                pos.x + x,
                pos.y,
                pos.z
            );
            Transform.position = pos;
        }

        /// <summary>
        /// ワールド座標における Y 座標に加算します
        /// </summary>
        public void AddPositionY( float y )
        {
            var pos = Transform.position;
            pos = new
            (
                pos.x,
                pos.y + y,
                pos.z
            );
            Transform.position = pos;
        }

        /// <summary>
        /// ワールド座標における Z 座標に加算します
        /// </summary>
        public void AddPositionZ( float z )
        {
            var pos = Transform.position;
            pos = new
            (
                pos.x,
                pos.y,
                pos.z + z
            );
            Transform.position = pos;
        }

        /// <summary>
        /// Vector2 型でワールド座標における位置を加算します
        /// </summary>
        public void AddPosition( Vector2 v )
        {
            var pos = Transform.position;
            pos = new
            (
                pos.x + v.x,
                pos.y + v.y,
                pos.z
            );
            Transform.position = pos;
        }

        /// <summary>
        /// Vector3 型でワールド座標における位置を加算します
        /// </summary>
        public void AddPosition( Vector3 v )
        {
            Transform.position += v;
        }

        /// <summary>
        /// ワールド座標における位置を加算します
        /// </summary>
        public void AddPosition( float x, float y )
        {
            var pos = Transform.position;
            pos = new
            (
                pos.x + x,
                pos.y + y,
                pos.z
            );
            Transform.position = pos;
        }

        /// <summary>
        /// ワールド座標における位置を加算します
        /// </summary>
        public void AddPosition( float x, float y, float z )
        {
            var pos = Transform.position;
            pos = new
            (
                pos.x + x,
                pos.y + y,
                pos.z + z
            );
            Transform.position = pos;
        }

        //================================================================================
        // ローカル座標における位置
        //================================================================================
        /// <summary>
        /// ローカル座標における位置をリセットします
        /// </summary>
        public void ResetLocalPosition()
        {
            Transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// ローカル座標における X 座標を設定します
        /// </summary>
        public void SetLocalPositionX( float x )
        {
            var pos = Transform.localPosition;
            pos = new
            (
                x,
                pos.y,
                pos.z
            );
            Transform.localPosition = pos;
        }

        /// <summary>
        /// ローカル座標における Y 座標を設定します
        /// </summary>
        public void SetLocalPositionY( float y )
        {
            var pos = Transform.localPosition;
            pos = new
            (
                pos.x,
                y,
                pos.z
            );
            Transform.localPosition = pos;
        }

        /// <summary>
        /// ローカル座標における Z 座標を設定します
        /// </summary>
        public void SetLocalPositionZ( float z )
        {
            var pos = Transform.localPosition;
            pos = new
            (
                pos.x,
                pos.y,
                z
            );
            Transform.localPosition = pos;
        }

        /// <summary>
        /// Vector2 型でローカル座標における位置を設定します
        /// </summary>
        public void SetLocalPosition( Vector2 v )
        {
            Transform.localPosition = new
            (
                v.x,
                v.y,
                Transform.localPosition.z
            );
        }

        /// <summary>
        /// Vector3 型でローカル座標における位置を設定します
        /// </summary>
        public void SetLocalPosition( Vector3 v )
        {
            Transform.localPosition = v;
        }

        /// <summary>
        /// ローカル座標における位置を設定します
        /// </summary>
        public void SetLocalPosition( float x, float y )
        {
            Transform.localPosition = new
            (
                x,
                y,
                Transform.localPosition.z
            );
        }

        /// <summary>
        /// ローカル座標における位置を設定します
        /// </summary>
        public void SetLocalPosition( float x, float y, float z )
        {
            Transform.localPosition = new
            (
                x,
                y,
                z
            );
        }

        /// <summary>
        /// ローカル座標における X 座標に加算します
        /// </summary>
        public void AddLocalPositionX( float x )
        {
            var pos = Transform.localPosition;
            pos = new
            (
                pos.x + x,
                pos.y,
                pos.z
            );
            Transform.localPosition = pos;
        }

        /// <summary>
        /// ローカル座標における Y 座標に加算します
        /// </summary>
        public void AddLocalPositionY( float y )
        {
            var pos = Transform.localPosition;
            pos = new
            (
                pos.x,
                pos.y + y,
                pos.z
            );
            Transform.localPosition = pos;
        }

        /// <summary>
        /// ローカル座標における Z 座標に加算します
        /// </summary>
        public void AddLocalPositionZ( float z )
        {
            var pos = Transform.localPosition;
            pos = new
            (
                pos.x,
                pos.y,
                pos.z + z
            );
            Transform.localPosition = pos;
        }

        /// <summary>
        /// Vector2 型でローカル座標における位置を加算します
        /// </summary>
        public void AddLocalPosition( Vector2 v )
        {
            var pos = Transform.localPosition;
            pos = new
            (
                pos.x + v.x,
                pos.y + v.y,
                pos.z
            );
            Transform.localPosition = pos;
        }

        /// <summary>
        /// Vector3 型でローカル座標における位置を加算します
        /// </summary>
        public void AddLocalPosition( Vector3 v )
        {
            Transform.localPosition += v;
        }

        /// <summary>
        /// ローカル座標における位置を加算します
        /// </summary>
        public void AddLocalPosition( float x, float y )
        {
            var pos = Transform.localPosition;
            pos = new
            (
                pos.x + x,
                pos.y + y,
                pos.z
            );
            Transform.localPosition = pos;
        }

        /// <summary>
        /// ローカル座標における位置を加算します
        /// </summary>
        public void AddLocalPosition( float x, float y, float z )
        {
            var pos = Transform.localPosition;
            pos = new
            (
                pos.x + x,
                pos.y + y,
                pos.z + z
            );
            Transform.localPosition = pos;
        }

        //================================================================================
        // ワールド座標における回転角
        //================================================================================
        /// <summary>
        /// ワールド座標における回転角をリセットします
        /// </summary>
        public void ResetEulerAngles()
        {
            Transform.eulerAngles = Vector3.zero;
        }

        /// <summary>
        /// ワールド座標における X 軸方向の回転角を設定します
        /// </summary>
        public void SetEulerAngleX( float x )
        {
            var angles = Transform.eulerAngles;
            angles = new
            (
                x,
                angles.y,
                angles.z
            );
            Transform.eulerAngles = angles;
        }

        /// <summary>
        /// ワールド座標における Y 軸方向の回転角を設定します
        /// </summary>
        public void SetEulerAngleY( float y )
        {
            var angles = Transform.eulerAngles;
            angles = new
            (
                angles.x,
                y,
                angles.z
            );
            Transform.eulerAngles = angles;
        }

        /// <summary>
        /// ワールド座標における Z 軸方向の回転角を設定します
        /// </summary>
        public void SetEulerAngleZ( float z )
        {
            var angles = Transform.eulerAngles;
            angles = new
            (
                angles.x,
                angles.y,
                z
            );
            Transform.eulerAngles = angles;
        }

        /// <summary>
        /// ワールド座標における回転角を設定します
        /// </summary>
        public void SetEulerAngles( Vector3 v )
        {
            Transform.eulerAngles = v;
        }

        /// <summary>
        /// ワールド座標における回転角を設定します
        /// </summary>
        public void SetEulerAngles( float x, float y, float z )
        {
            Transform.eulerAngles = new
            (
                x,
                y,
                z
            );
        }

        /// <summary>
        /// ワールド座標における X 軸方向の回転角を加算します
        /// </summary>
        public void AddEulerAngleX( float x )
        {
            Transform.Rotate( x, 0, 0, Space.World );
        }

        /// <summary>
        /// ワールド座標における Y 軸方向の回転角を加算します
        /// </summary>
        public void AddEulerAngleY( float y )
        {
            Transform.Rotate( 0, y, 0, Space.World );
        }

        /// <summary>
        /// ワールド座標における Z 軸方向の回転角を加算します
        /// </summary>
        public void AddEulerAngleZ( float z )
        {
            Transform.Rotate( 0, 0, z, Space.World );
        }

        //================================================================================
        // ローカル座標における回転角
        //================================================================================
        /// <summary>
        /// ローカル座標における回転角をリセットします
        /// </summary>
        public void ResetLocalEulerAngles()
        {
            Transform.localEulerAngles = Vector3.zero;
        }

        /// <summary>
        /// ローカル座標における X 軸方向の回転角を設定します
        /// </summary>
        public void SetLocalEulerAngleX( float x )
        {
            var angles = Transform.localEulerAngles;
            angles = new
            (
                x,
                angles.y,
                angles.z
            );
            Transform.localEulerAngles = angles;
        }

        /// <summary>
        /// ローカル座標における Y 軸方向の回転角を設定します
        /// </summary>
        public void SetLocalEulerAngleY( float y )
        {
            var angles = Transform.localEulerAngles;
            angles = new
            (
                angles.x,
                y,
                angles.z
            );
            Transform.localEulerAngles = angles;
        }

        /// <summary>
        /// ローカル座標における Z 軸方向の回転角を設定します
        /// </summary>
        public void SetLocalEulerAngleZ( float z )
        {
            var angles = Transform.localEulerAngles;
            angles = new
            (
                angles.x,
                angles.y,
                z
            );
            Transform.localEulerAngles = angles;
        }

        /// <summary>
        /// ローカル座標における回転角を設定します
        /// </summary>
        public void SetLocalEulerAngles( Vector3 v )
        {
            Transform.localEulerAngles = v;
        }

        /// <summary>
        /// ローカル座標における回転角を設定します
        /// </summary>
        public void SetLocalEulerAngles( float x, float y, float z )
        {
            Transform.localEulerAngles = new
            (
                x,
                y,
                z
            );
        }

        /// <summary>
        /// ローカル座標における X 軸方向の回転角を加算します
        /// </summary>
        public void AddLocalEulerAngleX( float x )
        {
            Transform.Rotate( x, 0, 0, Space.Self );
        }

        /// <summary>
        /// ローカル座標における Y 軸方向の回転角を加算します
        /// </summary>
        public void AddLocalEulerAngleY( float y )
        {
            Transform.Rotate( 0, y, 0, Space.Self );
        }

        /// <summary>
        /// ローカル座標における X 軸方向の回転角を加算します
        /// </summary>
        public void AddLocalEulerAngleZ( float z )
        {
            Transform.Rotate( 0, 0, z, Space.Self );
        }

        //================================================================================
        // ローカル座標におけるスケーリング値
        //================================================================================
        /// <summary>
        /// ローカル座標におけるスケーリング値をリセットします
        /// </summary>
        public void ResetLocalScale()
        {
            Transform.localScale = Vector3.one;
        }

        /// <summary>
        /// ローカル座標における X 軸方向のスケーリング値を設定します
        /// </summary>
        public void SetLocalScaleX( float x )
        {
            var scale = Transform.localScale;
            scale = new
            (
                x,
                scale.y,
                scale.z
            );
            Transform.localScale = scale;
        }

        /// <summary>
        /// ローカル座標における Y 軸方向のスケーリング値を設定します
        /// </summary>
        public void SetLocalScaleY( float y )
        {
            var scale = Transform.localScale;
            scale = new
            (
                scale.x,
                y,
                scale.z
            );
            Transform.localScale = scale;
        }

        /// <summary>
        /// ローカル座標における Z 軸方向のスケーリング値を設定します
        /// </summary>
        public void SetLocalScaleZ( float z )
        {
            var scale = Transform.localScale;
            scale = new
            (
                scale.x,
                scale.y,
                z
            );
            Transform.localScale = scale;
        }

        /// <summary>
        /// Vector2 型でローカル座標におけるスケーリング値を設定します
        /// </summary>
        public void SetLocalScale( Vector2 v )
        {
            Transform.localScale = new
            (
                v.x,
                v.y,
                Transform.localScale.z
            );
        }

        /// <summary>
        /// Vector3 型でローカル座標におけるスケーリング値を設定します
        /// </summary>
        public void SetLocalScale( Vector3 v )
        {
            Transform.localScale = v;
        }

        /// <summary>
        /// ローカル座標におけるスケーリング値を設定します
        /// </summary>
        public void SetLocalScale( float x, float y )
        {
            Transform.localScale = new
            (
                x,
                y,
                Transform.localScale.z
            );
        }

        /// <summary>
        /// ローカル座標におけるスケーリング値を設定します
        /// </summary>
        public void SetLocalScale( float x, float y, float z )
        {
            Transform.localScale = new
            (
                x,
                y,
                z
            );
        }

        /// <summary>
        /// ローカル座標における X 軸方向のスケーリング値を加算します
        /// </summary>
        public void AddLocalScaleX( float x )
        {
            var scale = Transform.localScale;
            scale = new
            (
                scale.x + x,
                scale.y,
                scale.z
            );
            Transform.localScale = scale;
        }

        /// <summary>
        /// ローカル座標における Y 軸方向のスケーリング値を加算します
        /// </summary>
        public void AddLocalScaleY( float y )
        {
            var scale = Transform.localScale;
            scale = new
            (
                scale.x,
                scale.y + y,
                scale.z
            );
            Transform.localScale = scale;
        }

        /// <summary>
        /// ローカル座標における Z 軸方向のスケーリング値を加算します
        /// </summary>
        public void AddLocalScaleZ( float z )
        {
            var scale = Transform.localScale;
            scale = new
            (
                scale.x,
                scale.y,
                scale.z + z
            );
            Transform.localScale = scale;
        }

        /// <summary>
        /// Vector2 型でローカル座標におけるスケーリング値を加算します
        /// </summary>
        public void AddLocalScale( Vector2 v )
        {
            var scale = Transform.localScale;
            scale = new
            (
                scale.x + v.x,
                scale.y + v.y,
                scale.z
            );
            Transform.localScale = scale;
        }

        /// <summary>
        /// Vector3 型でローカル座標におけるスケーリング値を加算します
        /// </summary>
        public void AddLocalScale( Vector3 v )
        {
            Transform.localScale += v;
        }

        /// <summary>
        /// ローカル座標におけるスケーリング値を加算します
        /// </summary>
        public void AddLocalScale( float x, float y )
        {
            var scale = Transform.localScale;
            scale = new
            (
                scale.x + x,
                scale.y + y,
                scale.z
            );
            Transform.localScale = scale;
        }

        /// <summary>
        /// ローカル座標におけるスケーリング値を加算します
        /// </summary>
        public void AddLocalScale( float x, float y, float z )
        {
            var scale = Transform.localScale;
            scale = new
            (
                scale.x + x,
                scale.y + y,
                scale.z + z
            );
            Transform.localScale = scale;
        }
    }
}