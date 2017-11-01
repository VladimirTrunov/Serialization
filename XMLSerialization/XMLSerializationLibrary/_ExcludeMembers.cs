using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLSerializationLibrary
{
    class _ExcludeMembers
    {
        private List<string> MemberNames { get; set; }
        public _ExcludeMembers() {
            MemberNames = new List<string>();
        }
        public _ExcludeMembers(List<string> memberNames)
        {
            this.MemberNames = memberNames;
        }

        public bool Contains(string memberName)
        {
            foreach(string str in MemberNames)
            {
                if (str == memberName)
                    return true;
            }

            return false;
        }

        public void Add(string memberName)
        {
            foreach (string str in MemberNames)
            {
                if (str == memberName)
                    throw new Exception("This member name '" + str + "' is already contained in ExcludeMembers");
            }

            MemberNames.Add(memberName);
        }

        public void Remove(string memberName)
        {
            bool isContains = false;
            foreach (string str in MemberNames)
            {
                if (str == memberName)
                    isContains = true;
            }

            if(isContains == false)
                throw new Exception("This member name '" + memberName + "' is not contained in ExcludeMembers");
            else
            {
                MemberNames.Remove(memberName);
            }
        }

        public void Clear()
        {
            MemberNames.Clear();
        }
    }
}
