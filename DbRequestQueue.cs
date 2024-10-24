using System.Collections.Generic;

namespace Pioneer
{
    //public class DbRequestQueue
    //{
    //    public Queue<IDbRequest> _queue = new Queue<IDbRequest>();
    //    private GameContentDbContext _db;

    //    public DbRequestQueue(GameContentDbContext db)
    //    {
    //        Debug.Assert(db != null);
    //        _db = db;
    //    }

    //    public void Flush()
    //    {
    //        if (_queue.Count == 0)
    //            return;

    //        while(_queue.Count > 0)
    //        {
    //            var request = _queue.Dequeue();
    //            request.Apply(_db);
    //        }
    //    }

    //    public void Enqueue(IDbRequest request)
    //    {
    //        _queue.Enqueue(request);
    //    }
    //}

    //public interface IDbRequest
    //{
    //    void Apply(GameContentDbContext db);
    //}


    //public struct CurrencyChangeRequest
    //{
    //    public ItemData currency;

    //    public void Apply(GameContentDbContext db)
    //    {
    //        db.Currency[currency.key] += currency.amount;
    //    }
    //}

    //public struct EnergyBuyRequest : IDbRequest
    //{
    //    public CurrencyChangeRequest energyAmount;
    //    public CurrencyChangeRequest gem;

    //    public void Apply(GameContentDbContext db)
    //    {
    //        energyAmount.Apply(db);
    //        gem.Apply(db);
    //    }
    //}
}
