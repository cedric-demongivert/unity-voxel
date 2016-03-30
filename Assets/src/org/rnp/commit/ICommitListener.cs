using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.rnp.commit
{
  public interface ICommitListener<T> where T : ICommitable<T>
  {
    void OnCommit(T commited);
    void OnUnbind(T commitable);
    void OnBind(T commitable);
  }
}
