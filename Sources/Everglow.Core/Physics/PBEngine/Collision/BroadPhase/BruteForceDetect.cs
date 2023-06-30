using Everglow.Commons.Physics.PBEngine.Collision.Colliders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Physics.PBEngine.Collision.BroadPhase
{
	/// <summary>
	/// 暴力遍历AABB的粗粒度碰撞检测算法
	/// </summary>
	public class BruteForceDetect : BroadPhase
    {
        private struct BruteForceEntry
        {
            public int ColliderId;
            public AABB BoundingBox;
            public bool IsDynamic;
        }
        private List<Collider2D> _colliders;
        private Dictionary<string, List<BruteForceEntry>> _groups;
        public BruteForceDetect(CollisionGraph graph) : base(graph)
        {
            _colliders  = new List<Collider2D>();
            _groups = new Dictionary<string, List<BruteForceEntry>>();
        }

        private void GroupInnerDetection(List<BruteForceEntry> entries, 
            List<KeyValuePair<Collider2D, Collider2D>> collisionResults)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                for (int j = i + 1; j < entries.Count; j++)
                {
                    if ((entries[i].IsDynamic || entries[j].IsDynamic) 
                        && entries[i].BoundingBox.Intersects(entries[j].BoundingBox))
                    {
                        collisionResults.Add(new KeyValuePair<Collider2D, Collider2D>(_colliders[entries[i].ColliderId],
                            _colliders[entries[j].ColliderId]));
                    }
                }
            }
        }

        private void GroupOuterDetection(List<BruteForceEntry> groupA,
            List<BruteForceEntry> groupB,
            List<KeyValuePair<Collider2D, Collider2D>> collisionResults)
        {
            for (int i = 0; i < groupA.Count; i++)
            {
                for (int j = 0; j < groupB.Count; j++)
                {
                    if ((groupA[i].IsDynamic || groupB[j].IsDynamic) 
                        && groupA[i].BoundingBox.Intersects(groupB[j].BoundingBox))
                    {
                        collisionResults.Add(new KeyValuePair<Collider2D, Collider2D>(_colliders[groupA[i].ColliderId],
                            _colliders[groupB[j].ColliderId]));
                    }
                }
            }
        }

        public override List<KeyValuePair<Collider2D, Collider2D>> GetCollisionPairs(float deltaTime)
        {
            List<KeyValuePair<Collider2D, Collider2D>> finalPairs = new List<KeyValuePair<Collider2D, Collider2D>>();
            foreach(var group in _groups)
            {
                var thisgroup = _groups[group.Key];
                GroupInnerDetection(thisgroup, finalPairs);
                if (_collisionGraph.Graph.ContainsKey(group.Key))
                {
                    foreach (var dual in _collisionGraph.Graph[group.Key])
                    {
                        if (!_groups.ContainsKey(dual))
                            continue;
                        GroupOuterDetection(thisgroup, _groups[dual], finalPairs);
                    }
                }
            }
            return finalPairs;
        }

        public override void Prepare(List<PhysicsObject> objects, float deltaTime)
        {
            _colliders.Clear();
            _colliders.EnsureCapacity(objects.Count);
            _groups.Clear();
            for(int i = 0; i < objects.Count; i++)
            {
                var obj = objects[i];
                _colliders.Add(obj.Collider);
                var entry = new BruteForceEntry()
                {
                    ColliderId = i,
                    BoundingBox = obj.Collider.GetAABB(deltaTime),
                    IsDynamic = obj.RigidBody.MovementType == MovementType.Dynamic || obj.RigidBody.MovementType == MovementType.Player
                };

                if (_groups.ContainsKey(obj.Tag))
                {
                    _groups[obj.Tag].Add(entry);
                }
                else
                {
                    _groups.Add(obj.Tag, new List<BruteForceEntry>()
                    {
                        entry
                    });
                }
            }
        }

    }
}
