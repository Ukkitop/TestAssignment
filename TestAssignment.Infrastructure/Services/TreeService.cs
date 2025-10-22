using Microsoft.EntityFrameworkCore;
using TestAssignment.Application.Interfaces;
using TestAssignment.Application.Models;
using TestAssignment.Domain.Entities;
using TestAssignment.Domain.Exceptions;
using TestAssignment.Infrastructure.Data;

namespace TestAssignment.Infrastructure.Services;

public class TreeService : ITreeService
{
    private readonly ApplicationDbContext _context;

    public TreeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MNode> GetTreeAsync(string treeName)
    {
        // Get or create root nodes for this tree
        var rootNodes = await _context.TreeNodes
            .Where(n => n.TreeName == treeName && n.ParentId == null)
            .Include(n => n.Children)
            .ToListAsync();

        // If tree doesn't exist, create a virtual root
        if (!rootNodes.Any())
        {
            return new MNode
            {
                Id = 0,
                Name = treeName,
                Children = new List<MNode>()
            };
        }

        // If there's only one root, return it
        if (rootNodes.Count == 1)
        {
            return await BuildNodeHierarchy(rootNodes[0]);
        }

        // If there are multiple roots, create a virtual parent
        var virtualRoot = new MNode
        {
            Id = 0,
            Name = treeName,
            Children = new List<MNode>()
        };

        foreach (var root in rootNodes)
        {
            virtualRoot.Children.Add(await BuildNodeHierarchy(root));
        }

        return virtualRoot;
    }

    public async Task CreateNodeAsync(string treeName, long? parentNodeId, string nodeName)
    {
        // Validate node name is not empty
        if (string.IsNullOrWhiteSpace(nodeName))
        {
            throw new SecureException("Node name cannot be empty");
        }

        // If parentNodeId is provided, verify it exists and belongs to the same tree
        if (parentNodeId.HasValue)
        {
            var parent = await _context.TreeNodes.FindAsync(parentNodeId.Value);
            if (parent == null)
            {
                throw new SecureException($"Parent node with ID {parentNodeId} not found");
            }

            if (parent.TreeName != treeName)
            {
                throw new SecureException($"Parent node belongs to a different tree");
            }

            // Check if sibling with same name exists
            var siblingExists = await _context.TreeNodes
                .AnyAsync(n => n.ParentId == parentNodeId && n.Name == nodeName && n.TreeName == treeName);

            if (siblingExists)
            {
                throw new SecureException($"A node with name '{nodeName}' already exists at this level");
            }
        }
        else
        {
            // Check if root node with same name exists
            var rootExists = await _context.TreeNodes
                .AnyAsync(n => n.ParentId == null && n.Name == nodeName && n.TreeName == treeName);

            if (rootExists)
            {
                throw new SecureException($"A root node with name '{nodeName}' already exists in this tree");
            }
        }

        var newNode = new TreeNode
        {
            TreeName = treeName,
            ParentId = parentNodeId,
            Name = nodeName,
            CreatedAt = DateTime.UtcNow
        };

        _context.TreeNodes.Add(newNode);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNodeAsync(long nodeId)
    {
        var node = await _context.TreeNodes
            .Include(n => n.Children)
            .FirstOrDefaultAsync(n => n.Id == nodeId);

        if (node == null)
        {
            throw new SecureException($"Node with ID {nodeId} not found");
        }

        // Check if node has children
        if (node.Children.Any())
        {
            throw new SecureException("You have to delete all children nodes first");
        }

        _context.TreeNodes.Remove(node);
        await _context.SaveChangesAsync();
    }

    public async Task RenameNodeAsync(long nodeId, string newNodeName)
    {
        if (string.IsNullOrWhiteSpace(newNodeName))
        {
            throw new SecureException("Node name cannot be empty");
        }

        var node = await _context.TreeNodes.FindAsync(nodeId);
        if (node == null)
        {
            throw new SecureException($"Node with ID {nodeId} not found");
        }

        // Check if sibling with same name exists
        var siblingExists = await _context.TreeNodes
            .AnyAsync(n => n.ParentId == node.ParentId && n.Name == newNodeName && n.TreeName == node.TreeName && n.Id != nodeId);

        if (siblingExists)
        {
            throw new SecureException($"A node with name '{newNodeName}' already exists at this level");
        }

        node.Name = newNodeName;
        await _context.SaveChangesAsync();
    }

    private async Task<MNode> BuildNodeHierarchy(TreeNode node)
    {
        var mNode = new MNode
        {
            Id = node.Id,
            Name = node.Name,
            Children = new List<MNode>()
        };

        // Load children recursively
        var children = await _context.TreeNodes
            .Where(n => n.ParentId == node.Id)
            .ToListAsync();

        foreach (var child in children)
        {
            mNode.Children.Add(await BuildNodeHierarchy(child));
        }

        return mNode;
    }
}

