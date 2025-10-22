using Microsoft.AspNetCore.Mvc;
using TestAssignment.Application.Interfaces;

namespace TestAssignment.API.Controllers;

/// <summary>
/// Represents entire tree API
/// </summary>
[ApiController]
[Route("api.user.tree")]
public class TreeController : ControllerBase
{
    private readonly ITreeService _treeService;

    public TreeController(ITreeService treeService)
    {
        _treeService = treeService;
    }

    /// <summary>
    /// Returns your entire tree. If your tree doesn't exist it will be created automatically.
    /// </summary>
    /// <param name="request">Request containing the tree name</param>
    /// <returns>The tree structure</returns>
    [HttpPost("get")]
    public async Task<IActionResult> GetTree([FromBody] GetTreeRequest request)
    {
        var tree = await _treeService.GetTreeAsync(request.TreeName);
        return Ok(tree);
    }

    /// <summary>
    /// Create a new node in your tree. You must to specify a parent node ID that belongs to your tree or dont pass parent ID to create tree first level node. A new node name must be unique across all siblings.
    /// </summary>
    /// <param name="request">Request containing tree name, parent node ID, and node name</param>
    /// <returns>Success response</returns>
    [HttpPost("node.create")]
    public async Task<IActionResult> CreateNode([FromBody] CreateNodeRequest request)
    {
        await _treeService.CreateNodeAsync(request.TreeName, request.ParentNodeId, request.NodeName);
        return Ok();
    }

    /// <summary>
    /// Delete an existing node and all its descendants
    /// </summary>
    /// <param name="request">Request containing the node ID to delete</param>
    /// <returns>Success response</returns>
    [HttpPost("node.delete")]
    public async Task<IActionResult> DeleteNode([FromBody] DeleteNodeRequest request)
    {
        await _treeService.DeleteNodeAsync(request.NodeId);
        return Ok();
    }

    /// <summary>
    /// Rename an existing node in your tree. A new name of the node must be unique across all siblings.
    /// </summary>
    /// <param name="request">Request containing the node ID and new node name</param>
    /// <returns>Success response</returns>
    [HttpPost("node.rename")]
    public async Task<IActionResult> RenameNode([FromBody] RenameNodeRequest request)
    {
        await _treeService.RenameNodeAsync(request.NodeId, request.NewNodeName);
        return Ok();
    }
}

public record GetTreeRequest(string TreeName);
public record CreateNodeRequest(string TreeName, long? ParentNodeId, string NodeName);
public record DeleteNodeRequest(long NodeId);
public record RenameNodeRequest(long NodeId, string NewNodeName);

