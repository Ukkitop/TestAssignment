using TestAssignment.Application.Models;

namespace TestAssignment.Application.Interfaces;

public interface ITreeService
{
    Task<MNode> GetTreeAsync(string treeName);
    Task CreateNodeAsync(string treeName, long? parentNodeId, string nodeName);
    Task DeleteNodeAsync(long nodeId);
    Task RenameNodeAsync(long nodeId, string newNodeName);
}

